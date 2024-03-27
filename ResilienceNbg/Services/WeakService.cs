using ResilienceNbg.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ResilienceNbg.Services;
 
public static class WeakService
{
    public static int Counter = 1;


    public static async Task<int>   GetConnectionInstance() 
    {
        Counter++;
        Console.WriteLine("Tries to execute GetConnectionInstance");
        await Task.Delay(10);
        if (Counter %4 !=0 )throw new BusinessTransientException();
        Console.WriteLine("Executes GetConnectionInstance successfully");
        return 1;
    }
}
 
