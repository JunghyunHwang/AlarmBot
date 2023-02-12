﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AlarmBot
{
	public static class Bot
	{
        private static readonly System.Timers.Timer NEW_PRODUCT_TIMER;
        private static readonly System.Timers.Timer TODAY_DRAW_TIMER;
        private static readonly List<Messenger> MESSENGERS;

        private static readonly int NUMBER_OF_MESSENGERS = 8;
        private static readonly int A_DAY_MILLISECONDS = 86400000;

        private static readonly int HOURS_TO_CHECK_NEW_PRODUCTS = 21;
        private static readonly int MINUTES_TO_CHECK_NEW_PRODUCTS = 55;
        private static readonly int HOURS_TO_CHECK_TODAY_DRAW = 21;
        private static readonly int MINUTES_TO_CHECK_TODAY_DRAW = 55;

        public static bool bIsRunning { get; private set; } = false;

        static Bot()
        {
            NEW_PRODUCT_TIMER = new System.Timers.Timer();
            TODAY_DRAW_TIMER = new System.Timers.Timer();
            MESSENGERS = new List<Messenger>(NUMBER_OF_MESSENGERS);
        }

        public static void Start()
        {
            Console.WriteLine("Start Alarm bot");
            if (bIsRunning)
            {
                Debug.Assert(false, "Bot is Already running");
                return;
            }

            NEW_PRODUCT_TIMER.Interval = A_DAY_MILLISECONDS;
            NEW_PRODUCT_TIMER.Elapsed += async (sender, e) => await checkNewProducts();
            NEW_PRODUCT_TIMER.AutoReset = true;

            TODAY_DRAW_TIMER.Interval = A_DAY_MILLISECONDS;
            TODAY_DRAW_TIMER.Elapsed += (sender, e) => setTodayDrawNotification();
            TODAY_DRAW_TIMER.AutoReset = true;

            setMessenger();
            startAllTimers();

            bIsRunning = true;
        }

        private static void setMessenger()
        {
            MESSENGERS.Add(new Telegram());

            Debug.Assert(MESSENGERS.Count == (int)EMessenger.Count);
        }

        private static void startAllTimers()
        {
            System.Timers.Timer newProductTime = new System.Timers.Timer();
            System.Timers.Timer todayDrawTime = new System.Timers.Timer();

            DateTime checkNewProductsTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, HOURS_TO_CHECK_NEW_PRODUCTS, MINUTES_TO_CHECK_NEW_PRODUCTS, 0);
            DateTime checkTodayDrawTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, HOURS_TO_CHECK_TODAY_DRAW, MINUTES_TO_CHECK_TODAY_DRAW, 0);

            if (DateTime.Now > checkNewProductsTime)
            {
                checkNewProductsTime = checkNewProductsTime.AddDays(1);
            }

            if (DateTime.Now > checkTodayDrawTime)
            {
                checkTodayDrawTime = checkTodayDrawTime.AddDays(1);
            }

            newProductTime.Interval = (checkNewProductsTime - DateTime.Now).TotalMilliseconds;
            newProductTime.Elapsed += (sender, e) => { NEW_PRODUCT_TIMER.Enabled = true; };
            newProductTime.Elapsed += async (sender, e) => { await checkNewProducts(); };

            newProductTime.Enabled = true;

            todayDrawTime.Interval = (checkTodayDrawTime - DateTime.Now).TotalMilliseconds;
            todayDrawTime.Elapsed += (sender, e) => { TODAY_DRAW_TIMER.Enabled = true; };
            todayDrawTime.Elapsed += (sender, e) => { setTodayDrawNotification(); };
            todayDrawTime.Enabled = true;
        }

        private static async Task checkNewProducts()
        {
            Console.WriteLine("##### check new products #####");
            List<ProductInfo> newProducts = await BrandManager.CheckNewProducts();

            Console.WriteLine($"new Products count: {newProducts.Count}");

            if (newProducts.Count > 0)
            {
                DB.InsertProducts(newProducts);
            }
        }

        private static void setTodayDrawNotification()
        {
            Console.WriteLine("##### set draw notification #####");
            List<ProductInfo> todayDrawProduct = DB.GetTodayDrawProducts();

            if (todayDrawProduct.Count == 0)
            {
                return;
            }

            Console.WriteLine($"Today draw count: {todayDrawProduct.Count}");

            foreach (var messenger in MESSENGERS)
            {
                messenger.SetNotification(todayDrawProduct);
            }

            DB.DeleteProducts(todayDrawProduct);
            
            foreach (var p in todayDrawProduct)
            {
                BrandManager.RemoveProduct(p);
            }
        }
    }
}
