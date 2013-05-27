using System;

namespace Algorithm_Tester
{
    class Program
    {
        static void Main( string[] args )
        {
            if ( args.Length == 0 ) { Console.WriteLine( "You need to provide an argument" ); }
            else
            {
                System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();
                switch ( (string)args.GetValue( 0 ) )
                {

                    case "-help":
                        Console.WriteLine( "provide the name of the algorithm you want to test and feed it with arguments." );
                        Console.WriteLine();
                        Console.WriteLine( "Options are:" );
                        Console.WriteLine( "-bitcount integer; which will tell you how many set bits the bitmask of the integer number contains." );
                        break;
                    case "-bitcount":
                        int v = Convert.ToInt32( args.GetValue( 1 ) );
                        int bitcount; // c accumulates the total bits set in v
                        _stopwatch.Start();
                        for ( bitcount = 0; v > 0; bitcount++ )
                        {
                            v &= v - 1; // clear the least significant bit set
                        }
                        _stopwatch.Stop();
                        Console.WriteLine( "Your integer contains {0:D} set bits", bitcount );
                        Console.WriteLine();
                        Console.WriteLine( "The operation took {0:D} milliseconds.", _stopwatch.ElapsedMilliseconds );
                        Console.WriteLine( "The operation took {0:D} ticks.", _stopwatch.ElapsedTicks );
                        Console.WriteLine( "Stop watch IsHighPrecision:" + System.Diagnostics.Stopwatch.IsHighResolution );
                        break;
                    case "-bitposition":
                        int leafposition = Convert.ToInt32( args.GetValue( 1 ) );
                        int leafposition_2 = leafposition;
                        int LeafCount;
                        _stopwatch.Start();

                        for ( LeafCount = 0; leafposition_2 > 0; LeafCount++ ) { leafposition_2 &= leafposition_2 - 1; };

                        int[] result = new int[LeafCount];
                        int arrayPosition = 0;

                        for ( int i = 0; i < LeafCount; i++ )
                        {
                            if ( (leafposition & (1 << i)) != 0 )
                            {
                                result[arrayPosition++] = i;
                            }
                        }
                        _stopwatch.Stop();
                        //Console.WriteLine( result.ToString() );
                        Console.WriteLine();
                        Console.WriteLine( "The operation took {0:D} milliseconds.", _stopwatch.ElapsedMilliseconds );
                        Console.WriteLine( "The operation took {0:D} ticks.", _stopwatch.ElapsedTicks );
                        Console.WriteLine( "Stop watch IsHighPrecision:" + System.Diagnostics.Stopwatch.IsHighResolution );
                        break;
                    default:
                        Console.WriteLine( "Please specify a valid command" );
                        break;
                }
            }
        }
    }
}
