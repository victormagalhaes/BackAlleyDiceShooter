using System;
using System.Collections.Generic;
using System.Linq;

namespace BackAlleyDiceShooter
{
	class MainClass
	{
		public static void Main(string[] args)
		{
            // First step of all, splash screen and Neil's presentation. This is to show user that this game is presented to someone.
            SetUpInitialScreen();
            GetNeilPresentation();

            // Initial variables that will be used throughout the game: money and bettypes. Still setup in order to initiate game.
            int totalAmountOfMoney = 200;
            string[] betTypes = { 
                "Big", 
                "Small", 
                "Odd", 
                "Even", 
                "All 1s", 
                "All 2s", 
                "All 3s",
                "All 4s", 
                "All 5s", 
                "All 6s", 
                "Double 1s", 
                "Double 2s", 
                "Double 3s", 
                "Double 4s", 
                "Double 5s", 
                "Double 6s", 
                "Any triples", 
                "4 or 17", 
                "5 or 16", 
                "6 or 15", 
                "7 or 14", 
                "8 or 13", 
                "9 or 12", 
                "10 or 11" 
            };

            // The user should have the right to get out of the game anytime he/she wants, so we need to make sure that he/she is playing every time a new bet is offered.
            bool stillPlaying = true;

			// Ok, here we start game loop
			// All game is basically consisted in some steps...
			// 1st get a bet
			// 2nd get the dices
			// 3rd get money for bet
			// 4th process the bet
			// 5th get financial status
			// 6th show situation
			// 7th check if player will play again
			// And it will begin again

            while (stillPlaying)
            {
                // Clear every time a new loop occours.
                Console.Clear();

                // Some UI to situate user about the game and user input of bet
                int chosenBet = SetUpBetsScreenAndGetBet(betTypes, totalAmountOfMoney);

                // UI to get how many dollars user is going to provide to bet
                int bet = GetBetAmount(chosenBet, totalAmountOfMoney, betTypes);

                // Rolling 3 random dices
                List<int> dices = RollDices();

                // Just show dices...
                ShowDices(dices);

                // Main function: Here we find out what to do with all data that we have until now
                int totalEarnings = ProcessBet(totalAmountOfMoney, chosenBet, dices, bet);


                // Here we show the result of the bet.
                // First some UI
                Console.Write($"Your had ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(totalAmountOfMoney);
                Console.ResetColor();
                Console.Write(" and you now have ");
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine(totalEarnings < 0 ? 0 : totalEarnings);
                Console.ResetColor();


                // Because if totalEarnings is what you have after proccess bet, so if it's less than the money that you had before bet so the player lost money.
                if (totalEarnings < totalAmountOfMoney) {
                    Console.Write($"Which means that you lost ");
					Console.ForegroundColor = ConsoleColor.Green;
                    // Ok this is tricky. If the difference between new amount and old amount is bigger than old amount that means this is a negative number, so we need to subtract new amount in order to get a positive difference
                    Console.Write(totalAmountOfMoney - totalEarnings > totalAmountOfMoney ? totalAmountOfMoney : totalAmountOfMoney - totalEarnings);
					Console.ResetColor();
                }
                // There's an option that they could win nothing. Just stay with the same amount.
                else if (totalEarnings == 0)
                {
                    Console.WriteLine($"Which means that you didn't win nor loose");
                }
                // and this is the scenarium where player wins
                else
                {
                    Console.Write($"Which means that you won ");
					Console.ForegroundColor = ConsoleColor.Green;
                    // We don't need here that trick because we assume that the player has a positive earning, bigger than the old amount
                    Console.Write(totalEarnings - totalAmountOfMoney);
					Console.ResetColor();
                }
                Console.ResetColor();

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();

                // This is the test if the player's still able to play. That is, have amount of money between 0 and 100,000
                if (totalEarnings > 0 && totalEarnings < 100000)
                {
                    totalAmountOfMoney = totalEarnings;
                }
                else
                {
                    // Or we need to check if player still wants to play and finish the game.
                    stillPlaying = CheckIfPlayerCanStillPlay(totalEarnings, stillPlaying);
                    totalAmountOfMoney = 200;
                }
            }
        }

        // Just a cool way to show dices.
        public static void ShowDices(List<int> dices)
        {
			Console.Clear();
			Console.WriteLine("==============================================================================\n");
			Console.WriteLine("Here we go! Your lucky dices are...\n");

			for (int i = 0; i < dices.Count; i++)
			{
				Console.WriteLine($"Dice number { i + 1 }: { dices[i] }");
			}

			Console.WriteLine($"\nSum of dices: { SumOfDices(dices) }");
			Console.WriteLine("\n==============================================================================\n");

			Console.WriteLine("\nLet's check now what you've got...");
        }

        // UI for checking about player's choice of keep playing or not
        public static bool CheckIfPlayerCanStillPlay(int totalEarnings, bool stillPlaying)
        {
            if (totalEarnings >= 100000)
			{
				Console.WriteLine("Oh no! You broke the bank, you lucky! Please, don't come back!");
				Console.WriteLine("But if you want, you can start a new game. Would you like to play again? [Y, n]");
                string input = Console.ReadLine();
                if (!(input.ToUpper() == "Y" && input.Length == 1 || input == ""))
                {
                    stillPlaying = false;
                }

            }
            else if (totalEarnings <= 0)
			{
                Console.WriteLine("Hmmm... Guess you don't have more money to continue.");
                Console.WriteLine("Would you like to play again? [Y, n]");
				string input = Console.ReadLine();
				if (!(input.ToUpper() == "Y" && input.Length == 1 || input == ""))
				{
					stillPlaying = false;
				}
			}

            return stillPlaying;
        }

        // UI for get bet value
        public static int GetBetAmount(int chosenBet, int totalAmountOfMoney, string[] betTypes)
        {
            Console.WriteLine($"Ok! You've selected '{ betTypes[chosenBet - 1] }'! How much would you like to bet?");
            return GetBetValueForRangedIntegers(1, totalAmountOfMoney);
        }

        // Pick bet by number and apply all game rules by bet.
        public static int ProcessBet(int totalAmountofMoney, int chosenBet, List<int> dices, int bet)
        {
            switch (chosenBet)
			{
                // Big
				case 1:
                    totalAmountofMoney += IsDicesBetweenAndNotTriples(11, 17, dices, bet);
                    break;

				// Small
				case 2:
                    totalAmountofMoney += IsDicesBetweenAndNotTriples(4, 10, dices, bet);
				    break;

                // Odd
				case 3:
                    totalAmountofMoney += IsAnOddNumber(dices, bet);
					
					break;

				// Even
				case 4:
                    totalAmountofMoney += IsAnEvenNumber(dices, bet);
					break;

				// All 1s
				case 5:
                    totalAmountofMoney += GetSpecificTriplesForNumber(1, dices, bet);
					break;

				// All 2s
				case 6:
					totalAmountofMoney += GetSpecificTriplesForNumber(2, dices, bet);
					break;

				// All 3s
				case 7:
                    totalAmountofMoney += GetSpecificTriplesForNumber(3, dices, bet);
					break;

				// All 4s
				case 8:
					totalAmountofMoney += GetSpecificTriplesForNumber(4, dices, bet);
					break;

				// All 5s
				case 9:
					totalAmountofMoney += GetSpecificTriplesForNumber(5, dices, bet);
					break;

				// All 6s
				case 10:
					totalAmountofMoney += GetSpecificTriplesForNumber(6, dices, bet);
					break;

				// Doubles 1s
				case 11:
                    totalAmountofMoney += GetSpecificDoublesForNumber(1, dices, bet);
					break;

				// Doubles 2s
				case 12:
					totalAmountofMoney += GetSpecificDoublesForNumber(2, dices, bet);
					break;

				// Doubles 3s
				case 13:
					totalAmountofMoney += GetSpecificDoublesForNumber(3, dices, bet);
					break;

				// Doubles 4s
				case 14:
					totalAmountofMoney += GetSpecificDoublesForNumber(4, dices, bet);
					break;

				// Doubles 5s
				case 15:
					totalAmountofMoney += GetSpecificDoublesForNumber(5, dices, bet);
					break;

				// Doubles 6s
				case 16:
					totalAmountofMoney += GetSpecificDoublesForNumber(6, dices, bet);
					break;

				// Any Triples
				case 17:
                    totalAmountofMoney += GetAnyTriples(dices, bet);
					break;

				// 4 or 17
				case 18:
                    totalAmountofMoney += IsSumOneOfTheseNumbers(4, 17, 60, dices, bet);
					break;

				// 5 or 16
				case 19:
                    totalAmountofMoney += IsSumOneOfTheseNumbers(5, 16, 30, dices, bet);
					break;

				// 6 or 15
				case 20:
                    totalAmountofMoney += IsSumOneOfTheseNumbers(6, 15, 18, dices, bet);
					break;

				// 7 or 14
				case 21:
                    totalAmountofMoney += IsSumOneOfTheseNumbers(7, 14, 12, dices, bet);
					break;

				// 8 or 13
				case 22:
					totalAmountofMoney += IsSumOneOfTheseNumbers(8, 13, 8, dices, bet);
					break;

				// 9 or 12
				case 23:
					totalAmountofMoney += IsSumOneOfTheseNumbers(9, 12, 7, dices, bet);
					break;

				// 10 or 11
				case 24:
					totalAmountofMoney += IsSumOneOfTheseNumbers(10, 11, 6, dices, bet);
					break;
			}

            return totalAmountofMoney;
		}

        // Basically we take the sum of dices, and check if the sum is whether one number of another.
        // Odds are passed through parameter so we could reuse this function every time we need.
        public static int IsSumOneOfTheseNumbers(int firstNumber, int secondNumber, int odds, List<int> dices, int bet)
        {
			int sumOfDices = SumOfDices(dices);

            if (sumOfDices == firstNumber || sumOfDices == secondNumber)
			{
				return bet * odds;
			}

			return bet * odds * (-1);
        }

        // Call another function that returns if it's triples or not but return odds.
        public static int GetAnyTriples(List<int> dices, int bet)
        {
			if (IsTriples(dices))
			{
				return bet * 30;
			}
			return bet * (-30);
        }

        // Call function that returns if there is 2 doubles or not and return odds
        public static int GetSpecificDoublesForNumber(int number, List<int> dices, int bet)
        {
			if (IsSpecificDoubles(dices, number) && !IsTriples(dices))
			{
				return bet * 10;
			}
            return bet * (-10);
        }

        // Same. Check if there's triples and return odds
        public static int GetSpecificTriplesForNumber(int number, List<int> dices, int bet)
        {
			if (IsSpecificTriples(dices, number))
			{
				return bet * 180;
			}

            return bet * (-180);
        }

        // Return odds if is even number
        public static int IsAnEvenNumber(List<int> dices, int bet)
        {
			int sumOfDices = SumOfDices(dices);

			// Get what rest of a division of number by 2. Exception of triples.
			if (sumOfDices % 2 == 0 && !IsTriples(dices))
			{
				return bet;
			}

			return bet * (-1);
        }

        // return odds if is odd number. 
        public static int IsAnOddNumber(List<int> dices, int bet)
        {
			int sumOfDices = SumOfDices(dices);

			// Get what rest of a division of number by 2. Exception of triples.
			if (sumOfDices % 2 != 0 && !IsTriples(dices))
			{
				return bet;
			}

            return bet * (-1);
		}

        // Check if sum is between 2 numbers, used in big and small bets. Exception if triples. Return odds.
        public static int IsDicesBetweenAndNotTriples(int min, int max, List<int> dices, int bet)
        {
            int sumOfDices = SumOfDices(dices);

			if (sumOfDices >= min && sumOfDices <= max && !IsTriples(dices))
			{
				return bet;
			}

            return bet * (-1);
        }

        // True or false. Is there 2 doubles? Exception of Triples.
        public static bool IsSpecificDoubles(List<int> dices, int number)
        {
            // Basically count how many items of number there is in list and if it's not a triple.
            return dices.Count(x => x == number) == 2 && !IsTriples(dices);
        }

        // Verify if the first number is equals to second and if second equals third.
        public static bool IsTriples(List<int> dices)
        {
            return dices[0] == dices[1] && dices[1] == dices[2];
        }

        // Looks like the orher one, but with one small change: every number must be a parameter number.
        public static bool IsSpecificTriples(List<int> dices, int number)
		{
			return dices[0] == number && dices[1] == number && dices[2] == number;
		}

        // Get a list of 3 random numbers between 1 and 6
        public static List<int> RollDices()
		{
            List<int> dices = new List<int>();
            Random number = new Random();

            for (int i = 0; i < 3; i++)
            {
            	dices.Add(number.Next(6) + 1);
            }

			return dices;
		}

        // Get the list of dices and make a sum of it
        public static int SumOfDices(List<int> dices)
        {
            int sum = 0;
            foreach (var dice in dices)
            {
                sum += dice;
            }
            return sum;
        }

        // UI of initial questions. Show bets and amount of money.
        public static int SetUpBetsScreenAndGetBet(string[] betTypes, int amountOfMoney)
		{
            Console.Write("You've got ");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write($"{ amountOfMoney} ");
			Console.ResetColor();
			Console.Write("dollars in your pocket and here's our bets:\n");
			Console.WriteLine("\n==============================================================================\n");
            for(int i = 0; i < betTypes.Length; i++)
			{
                // Naming bets of list of bettypes
                string betName = $"{ i + 1 }. { betTypes[i] }"; 
                // Use 20 columns to place bet name.
                Console.Write($"{ betName, -20 }");
			}
			Console.WriteLine("\n==============================================================================\n");
            Console.WriteLine($"What bet would you like to make? (Pick a number between 1 and { betTypes.Length })");

            return GetBetValueForRangedIntegers(1, betTypes.Length);
		}

        // Just another UI for
        public static int GetBetValueForRangedIntegers(int min, int max)
        {
            bool isValid = false;
            int bet = 0;

            // While the input is not a number between 1 and 24 (number of bettypes)...
            while (!isValid)
            {
                // get input from user.
                string input = Console.ReadLine();
                if (int.TryParse(input, out bet))
                {
					if (bet < min || bet > max)
					{
                        Console.WriteLine($"There's something wrong here! Pick a number between { min } and { max }.");
						isValid = false;
                    }
                    else
                    {
                        isValid = true;
                    }
                }
                else
                {
					Console.WriteLine($"There's something wrong here! Pick a number between { min } and { max }.");
					isValid = false;
                }
            }

            return bet;
        }

        // It's a TV Show! There must be a showman. Who's better than Neil Patrick Harris for this job?
        public static void GetNeilPresentation()
        {
            Console.WriteLine("Hi! I'm Neil Patrick Harris!");
            Console.WriteLine("You probably know me by my role in 'How I met your mother'!");
            Console.WriteLine("But I'm also a great magician!");
            Console.WriteLine("I'll make your money multiply here at my new TV show 'Easy Bet'!\n");
        }

        // Splash screen
		public static void SetUpInitialScreen()
		{
			Console.WriteLine("===============================================================================");
			Console.WriteLine("                                     |                                         ");
			Console.WriteLine("                 $$$                 |                                         ");
			Console.WriteLine("                 $$$                 |           XXXX  XXXX  XXXX X  X         ");
			Console.WriteLine("              $$$$$$$$$              |           X     X  X  X    X  X         ");
			Console.WriteLine("           $$$$  $$$  $$$$           |           XXXX  XXXX  XXXX  XX          ");
			Console.WriteLine("         $$$     $$$     $$$         |           X     X  X     X  XX          ");
			Console.WriteLine("         $$$     $$$      $$         |           XXXX  X  X  XXXX  XX          ");
			Console.WriteLine("         $$$     $$$                 |                                         ");
			Console.WriteLine("           $$$$  $$$                 |                                         ");
			Console.WriteLine("             $$$$$$$                 |                                         ");
			Console.WriteLine("                $$$$$$$              |                                         ");
			Console.WriteLine("                 $$$  $$$$           |              XXX  XXXX XXXX             ");
			Console.WriteLine("                 $$$     $$$         |              X  X X     XX              ");
			Console.WriteLine("         $$      $$$     $$$         |              XXX  XXXX  XX              ");
			Console.WriteLine("         $$$     $$$     $$$         |              X  X X     XX              ");
			Console.WriteLine("           $$$$  $$$  $$$$           |              XXX  XXXX  XX              ");
			Console.WriteLine("              $$$$$$$$$              |                                         ");
			Console.WriteLine("                 $$$                 |                                         ");
			Console.WriteLine("                 $$$                 |          by Neil Patrick Harris         ");
			Console.WriteLine("                                     |                                         ");
			Console.WriteLine("===============================================================================");
			Console.WriteLine("                          Press any key to start.                              ");
			Console.ReadKey();
			Console.Clear();
		}
	}
}
