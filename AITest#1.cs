namespace AITest
{
    static class Start
    {

        public static async Task<int> Main(string[] args)
        {
            AIHandler handler = new AIHandler();
            Console.WriteLine("Hello World!\n");
            while (true)
            {
                string enter = Input("Retry? Y/n/(number)/r/p/e: ");

                switch (enter)
                {
                    case "N": //No to leave
                    case "n": return 0;
                    case "Y": //Yes for manual input
                    case "y": handler.MakeAI(); break;
                    case "R": //R for reload and making  a new Handler with the Inputted "braincells" (Braincells in Brackeys, bc techicly they are not braincells, but I explain that in another comment it in more details)
                    case "r": int newCells = Convert.ToInt16(Input("Number of braincells: ")); handler = new AIHandler(newCells); break;
                    case "P": //P for print the first two Braincells
                    case "p": Console.WriteLine("Braincells: " + handler.BestAI.braincellMutation[0] + " & " + handler.BestAI.braincellMutation[1]); break;
                    case "E": //E for (~~EA~~ {Pls imagine .md Textforamt}) Experiment
                    case "e": await Experiment(); break;
                    //Or write a number to make a specific amount of calculations with Random Numbers
                    default: for (int i = 0; i < Convert.ToInt16(enter); i++) { Console.Write("\n" + (i + 1) + ":"); handler.MakeAI(new Random().Next(-10, 10), new Random().Next(-10, 10)); } break;
                }
                //if (Input("Retry? Y/n") == "n") break;
            }
            return 01;//Something went wrong if you reach this -> Error Code: 1
        }

        public static async Task Experiment() //Ah btw. Doing this it, the pc will make maximum 2500 calculations per "Braincell"
        {
            int MaxBraincells = Convert.ToInt16(Input("Max Braincells: "));
            if (MaxBraincells < 2)
            {
                Console.WriteLine("You need a minimum of 2 Braincells! idiot...");
                return;
            }
            float[] averageTrys = new float[MaxBraincells + 1];
            int average = 10;

            //This is the same code as "Multi Threading #1", this one is apperently more efficnet but not working and I rn dont care to fixing it
            //Multi Threading #2

            //Task[] taskArray = new Task[MaxBraincells + 1];
            //for (int i = 2; i < taskArray.Length-1; i++)
            //{
            //    taskArray[i] = Task.Factory.StartNew(() =>
            //    {
            //        int sum = 0;
            //        for (int j = 0; j < average; j++) //How often to get avrage
            //        {
            //            int RoundCounts = 0;
            //            int row = 0;
            //            for (int k = 0; k < 2500; k++)  //Trys
            //            {
            //                AIHandler handler = new AIHandler(i);
            //                Console.Write("\n" + (k + 1) + ":");
            //                handler.MakeAI(new Random().Next(-10, 10), new Random().Next(-10, 10));
            //                if (handler.BestAI.ResultHitChance == 1) row++;
            //                if (row == 3)
            //                {
            //                    RoundCounts = k;
            //                    break;
            //                }
            //            }
            //            sum += RoundCounts;
            //        }
            //        averageTrys[i] = ((float)sum / (float)average);
            //    });
            //}
            //Task.WaitAll(taskArray);

            //Multi Threading #1; It works just a litle bit better than SingleThread, probably becuase all of the Console.Write it needs to do, but again, not for now

            bool finished = Parallel.For(2, MaxBraincells + 1,
                async i =>
                {
                    int sum = 0;
                    for (int j = 0; j < average; j++) //How often to get avrage
                    {
                        int RoundCounts = 0;
                        int row = 0;
                        for (int k = 0; k < 2500; k++)  //Trys
                        {
                            AIHandler handler = new AIHandler(i);
                            Console.Write("\n" + (k + 1) + ":");
                            handler.MakeAI(new Random().Next(-10, 10), new Random().Next(-10, 10));
                            if (handler.BestAI.ResultHitChance == 1) row++;
                            if (row == 3)
                            {
                                RoundCounts = k;
                                break;
                            }
                        }
                        sum += RoundCounts;
                    }
                    averageTrys[i] = ((float)sum / (float)average); //Do you see here a Problem? what happens if the Results is: [0,0,0,0,1756,0,0,0,0,0], the Result will be 175.6, even though it should be 1756, because the others are not there, but again, I dont want to fix it, maybe you can try it! (Shouldbe a 2 Minute Problem btw.!)
                }).IsCompleted;
            if (true == finished)
            {
                Console.WriteLine("lol");
            }

            //Single Thread
            //for (int i = 2; i < MaxBraincells; i++) //Braincells
            //{
            //    int sum = 0;
            //    for (int j = 0; j < average; j++) //How often to get avrage
            //    {
            //        int RoundCounts = 0;
            //        int row = 0;
            //        for (int k = 0; k < 1000; k++)  //Trys
            //        {
            //            AIHandler handler = new AIHandler(i);
            //            Console.Write("\n" + (k + 1) + ":");
            //            handler.MakeAI(new Random().Next(-10, 10), new Random().Next(-10, 10));
            //            if (handler.BestAI.ResultHitChance == 1) row++;
            //            if (row == 3)
            //            {
            //                RoundCounts = k;
            //                break;
            //            }
            //        }
            //        sum += RoundCounts;
            //    }
            //    averageTrys[i] = ((float)sum / (float)average);
            //}

            //print results:

            Console.WriteLine("\n");
            Console.WriteLine($"Inserted Parameter: \nMaxBraincells: {MaxBraincells}");
            for (int i = 2; i < MaxBraincells + 1; i++)
            {
                if (averageTrys[i] != 0)
                    Console.WriteLine("Braincells: " + i + " Average Trys: " + averageTrys[i]);
                else Console.WriteLine("Braincells: " + i + " Average Trys: NaN");
            }
            Console.WriteLine("\n");
        }

        public static string Input(string input = "")
        {
            Console.Write(input);
            return Console.ReadLine();
        }
    }

    public class AIHandler
    {
        public AI BestAI = new AI(0, 0, 0, 0, new float[2], 2);
        int braincells = 2;


        public AIHandler(int bBraincells = 2)
        {
            if (bBraincells < 2)
            {
                Console.WriteLine("You need a minimum of 2 Braincells! idiot...");
                return;
            }
            braincells = bBraincells;
            BestAI = new AI(0, 0, 0, 0, new float[braincells], braincells);
        }

        public void MakeAI(int c = -11, int d = -11)
        {
            int a = 0;
            int b = 0;

            if (c == -11 && d == -11)
            {
                a = Convert.ToInt16(Start.Input("A: "));
                b = Convert.ToInt16(Start.Input("B: "));
            }
            else
            {
                a = c;
                b = d;
            }

            if ((a + b) == 0) a += 1; //Eliminate 0, since this breaks the Check algorythm!

            AI[] ais = new AI[5]; //Decleare Ai
            ais[0] = new AI(a, b, a + b, 0, BestAI.braincellMutation, braincells);//Create last AI //Why do I not just put the Old AI there? Idk and idc

            for (int i = 1; i < ais.Length; i++)
            {
                ais[i] = new AI(a, b, a + b, i, BestAI.braincellMutation, braincells); //Others AI
            }
            Console.WriteLine("\nExpected Result: " + (a + b) + " Braincells: " + ais[0].braincellMutation.Length);

            float highestResultChance = 0;
            int aiNumber = -1;

            for (int i = 0; i < ais.Length; i++)
            {
                if (highestResultChance < ais[i].ResultHitChance)
                {
                    highestResultChance = ais[i].ResultHitChance;
                    BestAI = ais[i]; //Apperantly it is more efficent, bc it needs to do less memory alorocation
                    aiNumber = i;
                }
                Console.WriteLine("Ai: " + i + " His Result: " + ais[i].actualResult + " Result Percentage: " + (ais[i].ResultHitChance * 100) + "%");
            }

            Console.WriteLine("\nBest AI: " + aiNumber + " With: " + highestResultChance * 100 + "%");
        }
    }
    public class AI
    {
        int a;
        int b;
        int ExpectedResult;
        public float[] braincellMutation;
        public float actualResult;
        public float ResultHitChance;

        public AI(int a, int b, int Result, int Mutation, float[] newBrain, int braincells)
        {
            this.a = a;
            this.b = b;
            ExpectedResult = Result;
            braincellMutation = new float[braincells];
            if (Mutation != 0)
            {
                Random r = new Random(Mutation);

                for (int i = 0; i < braincellMutation.Length; i++)
                {
                    braincellMutation[i] = newBrain[i] + (float)(r.Next(-Mutation, Mutation)) / 10f;
                }
            }
            else
            {
                braincellMutation = newBrain;
            }

            Calc();
        }

        public void Calc()
        {
            //actualResult = (a * braincellMutation[0]) + (braincellMutation[1] * b);//The actually AI btw
            for (int i = 0; i < braincellMutation.Length; i++)
            {
                switch (i % 2)
                {
                    case 0: actualResult += braincellMutation[i] * a; break;
                    case 1: actualResult += braincellMutation[i] * b; break;
                }
            }
            //Some Crazy Math: https://math.stackexchange.com/questions/1525331/calculate-percentage-of-how-far-a-number-is-away-from-a-given-point-in-a-range
            float d = based(ExpectedResult) + based(actualResult);
            ResultHitChance = based(d - based(ExpectedResult - actualResult)) / d; //ExpectedResult / actualResult; //Old fanishod, but I got math that makes it better
            //while (ResultHitChance > 1) ResultHitChance -= 1;
        }
        public float based(float a) //It is called based, bc base is a C# keyword and I have a Feeling, that the 1:1 translation from German Math "Betrag" means "Base". Or atleast I hope so
        {
            return a < 0 ? -a : a;
        }

    }
}
