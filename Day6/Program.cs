using InputLogic;

string input = (await InputHelper.GetInputAsync(day: 6)).First();

const int simulationLength = 256;
const int newFishTimer = 8;
const int resetTimer = 6; 
long[] fish = new long[9];

foreach (int timer in input.Split(',').Select(int.Parse))
{
    fish[timer]++;
}

for (int i = 0; i < simulationLength; i++)
{
    long readyFish = fish[0];
    
    for (int timer = 0; timer < newFishTimer;)
    {
        fish[timer] = fish[++timer];
    }

    fish[resetTimer] += readyFish;
    fish[newFishTimer] = readyFish;
}

Console.WriteLine(fish.Sum());