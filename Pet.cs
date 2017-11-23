﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Globalization;

namespace VirtualPets
{
    public enum Type { Cat, Dog, Rabbit, Turtle, Parrot, Horse };

    class Pet
    {
        // Instantiate variables that are needed throughout execution

        // Timers are a built-in Class that execute specified functions after an interval
        // https://msdn.microsoft.com/en-us/library/system.timers.timer(v=vs.110).aspx
        private static readonly Timer Tasks = new Timer();
        public static readonly int UpdateInterval = 7500;
        private static readonly Random RNG = new Random();

        // Declare a name
        private string name;
        public string Name
        {
            get
            {
                // This Class enabled me to convert strings to Title Case
                // https://msdn.microsoft.com/en-us/library/system.globalization.textinfo.totitlecase(v=vs.110).aspx
                TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
                return textInfo.ToTitleCase(name);
            }
            set
            {
                name = value;
            }
        }

        // Declare a Pet Type
        public Type Type;

        // Set the initial hunger level to 0
        internal int hunger = 0;
        // Declare a setter and getter for hunger
        public int Hunger
        {
            get
            {
                return hunger;
            }
            set
            {
                // Check that hunger does not decrease below 0
                hunger = value;
                if (hunger < 0) hunger = 0;
            }
        }

        // Set the initial boredom level to 0
        internal int boredom = 0;
        // Declare a setter and getter for boredom
        public int Boredom
        {
            get
            {
                return boredom;
            }
            set
            {
                // Check that boredom does not decrease below 0
                boredom = value;
                if (boredom < 0) boredom = 0;
            }
        }

        // Declare a getter for the Mood
        public string Mood
        {
            get
            {
                // Mood is based on Hunger + Boredom
                int moodFactor = Hunger + Boredom;

                if (moodFactor < 5) return "Extremely Happy";
                else if (moodFactor < 10) return "Happy";
                else if (moodFactor < 25) return "Content";
                else if (moodFactor < 50) return "Unhappy";
                else if (moodFactor < 75) return "Mad";
                else if (moodFactor < 95) return "Raging Mad";
                else return "Passed Out";
            }
        }

        // Define a method to initiate timers
        private void InitTimers()
        {
            // Set the timer's interval to 'UpdateInterval'
            Tasks.Interval = UpdateInterval;

            // Declare which methods should be called every time the timer lapses
            Tasks.Elapsed += new ElapsedEventHandler(IncreaseHunger);
            Tasks.Elapsed += new ElapsedEventHandler(IncreaseBoredom);
            Tasks.Elapsed += new ElapsedEventHandler(CheckDeathStatus);

            // Enable the timer
            Tasks.Enabled = true;
        }

        private void CheckDeathStatus(object source, ElapsedEventArgs e)
        {
            if (Mood == "Passed Out")
            {
                Program.CurrentlyInMenu = false;
                Program.GameOver = true;
                Tasks.Enabled = false;

                Program.ClearConsole();
                Console.Write($"It seems that your {Type} {Name} has passed out! The game is over. Do you want to restart? (y/n)\n > ");
                if (Console.ReadLine().ToLower().StartsWith("y"))
                {
                    Program.ClearConsole();
                    Program.Main(new String[0]);
                }
                else Environment.Exit(0);
            }
        }

        // Declare the default constructor
        public Pet(string name, int type)
        {
            Name = name;
            Type = (Type)type;

            InitTimers();
        }
        
        // Declare an overload constructor with initial values for hunger and boredom
        public Pet(string name, int type, int h, int b)
        {
            // Set the appropriate properties
            Name = name;
            Type = (Type)type;

            Hunger = h;
            Boredom = b;

            InitTimers();
        }

        // Nicely formatted string that displays necessary data about this pet
        public string GetStatusWindow()
        {
            return $"Name: {Name}\nType: {Type}\nHunger: {Hunger}\nBoredom: {Boredom}\nMood: {Mood}";
        }

        // The method to talk
        // Outputs a string based on the type of pet
        public void Talk()
        {
            switch (Type)
            {
                case (Type)0:
                    Console.WriteLine("Meow");
                    break;
                case (Type)1:
                    Console.WriteLine("Woof");
                    break;
                case (Type)2:
                    Console.WriteLine("[Rabbit Noises]");
                    break;
                case (Type)3:
                    Console.WriteLine("[Turtle Noises]");
                    break;
                case (Type)4:
                    Console.WriteLine("Ca-caw! You're funny!");
                    break;
                case (Type)5:
                    Console.WriteLine("Weugh");
                    break;
            }
        }

        // A method that is called every x seconds that increases the pet's hunger level
        internal void IncreaseHunger(object source, ElapsedEventArgs e)
        {
            Hunger += RNG.Next(5);
        }

        // Default method for feeding the pet with a default feeding level
        // Returns the updated 'Hunger' value
        public int Eat()
        {
            Hunger -= 5;
            return Hunger;
        }

        // Overload method for feeding the pet with a random value
        // Returns the updated 'Hunger' value
        public int Eat(bool random)
        {
            if (!random) return this.Eat();
            else Hunger -= RNG.Next(3, 8);
            return Hunger;
        }

        // Overload method for feeding the pet with a custom feeding level
        // Returns the updated 'Hunger' value
        public int Eat(int amount)
        {
            Hunger -= amount;
            return Hunger;
        }

        // A method that is called every x seconds that increases the pet's boredom level
        internal void IncreaseBoredom(object source, ElapsedEventArgs e)
        {
            Boredom += RNG.Next(5);
        }

        // Default method for decreasing the pet's boredom level with a default value
        // Returns the updated 'Boredom' value
        public int Play()
        {
            Boredom -= 5;
            return Boredom;
        }
        
        // Overload method for decreasing the pet's boredom level with a random value
        // Returns the updated 'Boredom' value
        public int Play(bool random)
        {
            if (!random) return this.Play();
            else Boredom -= RNG.Next(3, 8);
            return Boredom;
        }

        // Overload method for decreasing the pet's boredom level with a custom value
        // Returns the updated 'Boredom' value
        public int Play(int amount)
        {
            Boredom -= amount;
            return Boredom;
        }
    }
}