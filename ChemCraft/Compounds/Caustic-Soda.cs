using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChemCraft
{
    public class NaOH : Compound
    {

        public NaOH()
        {
            elementnum = 3;
            state = 0;
            damage = 4;
            name = "Casutic Soda";
            formula = "NaOH";
            type = Type.Undetermined;
            elements = new int[3] { 11, 8, 1 };
        }

        /// <summary>
        ///the compounds ability
        /// </summary>
       override public void ability()
        {

        }
        
    }
}
