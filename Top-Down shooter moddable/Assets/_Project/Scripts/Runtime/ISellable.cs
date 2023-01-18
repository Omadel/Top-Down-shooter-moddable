using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


interface ISellable
{
    int value { get; }
    int vegetableCount { get; }

    public void Sell(int vegetableAmount, int vegetableValue);
    public void EmptyBox();
    

}

