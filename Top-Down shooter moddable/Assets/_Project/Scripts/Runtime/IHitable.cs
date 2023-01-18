using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


interface IHitable
{
    int Health { get; }
    void Hit(int amount);
}

