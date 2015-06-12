using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChemCraft;

namespace ChemCraft
{
    public partial class Crucible : Form
    {
        private List<List<Element>> elements = new List<List<Element>>();
        private Player player;
        private List<Element> hand;
        private Deck deck;
        private List<Compound> compounds;
        private List<Compound> newCompounds;

        private int energy;

        public Crucible(Player myPlayer)
        {
            //initialize variables
            player = myPlayer;

            hand = player.Hand;
            deck = player.Deck;
            compounds = player.Compounds;
            energy = player.Energy;

            newCompounds = new List<Compound>();

            for(int i = 0; i < 118; i++)
            {
                elements.Add(new List<Element>());
            }

            InitializeComponent();

            //update
            updateElements();
            updateExistComp();
            updateNewComp();
        }

        private void updateElements()
        {
            //clear variables
            comboBoxElements.Items.Clear();

            for (int i = 0; i < hand.Count; i++)
            {
                //add the elements to the menu
                comboBoxElements.Items.Add(hand[i].elementSymbol);
                //add the elements to the array
                elements[hand[i].atomicNumber-1].Add(hand[i]);
            }
        }
       
        private void updateNewComp()
        {
            if (elements[11 - 1].Count >= 1 && elements[1 - 1].Count >= 1 && elements[6 - 1].Count >= 1 && elements[8 - 1].Count >= 3)
            {
                newCompounds.Add(new NaHCO3());
            }
            if (elements[11 - 1].Count >= 1 && elements[17 - 1].Count >= 1 && elements[8 - 1].Count >= 1)
            {
                newCompounds.Add(new NaClO());
            }
            if (elements[7 - 1].Count >= 2 && elements[8 - 1].Count >= 1)
            {
                newCompounds.Add(new N2O());
            }
            if (elements[20 - 1].Count >= 1 && elements[6 - 1].Count >= 1 && elements[8 - 1].Count >= 3)
            {
                newCompounds.Add(new CaCO3());
            }
            if (elements[8 - 1].Count >= 3 && elements[1 - 1].Count >= 8 && elements[8 - 1].Count >= 1)
            {
                newCompounds.Add(new C3H8O());
            }
            if (elements[6 - 1].Count >= 12 && elements[1 - 1].Count >= 22 && elements[8 - 1].Count >= 11)
            {
                newCompounds.Add(new C12H22O11());
            }
            if (elements[14 - 1].Count >= 1 && elements[8 - 1].Count >= 2)
            {
                newCompounds.Add(new SiO2());
            }
            if (elements[1 - 1].Count >= 1 && elements[17 - 1].Count >= 1)
            {
                newCompounds.Add(new HCl());
            }
            if (elements[11 - 1].Count >= 1 && elements[17 - 1].Count >= 1)
            {
                newCompounds.Add(new NaCl());
            }
            if (elements[11 - 1].Count >= 1 && elements[8 - 1].Count >= 1 && elements[1 - 1].Count >= 1)
            {
                newCompounds.Add(new NaOH());
            }
            if (elements[6 - 1].Count >= 8 && elements[1 - 1].Count >= 9 && elements[7 - 1].Count >= 1 && elements[8 - 1].Count >= 2)
            {
                newCompounds.Add(new C8H9NO2());
            }
            if (elements[1 - 1].Count >= 2 && elements[8 - 1].Count >= 1)
            {
                newCompounds.Add(new H2O());
            }
            if (elements[1 - 1].Count >= 2 && elements[8 - 1].Count >= 2)
            {
                newCompounds.Add(new H2O2());
            }
            if (elements[19 - 1].Count >= 1 && elements[8 - 1].Count >= 1 && elements[1 - 1].Count >= 1)
            {
                newCompounds.Add(new KOH());
            }

            comboBoxNewComp.Items.Clear();
            for (int i = 0; i < newCompounds.Count; i++)
            {
                comboBoxNewComp.Items.Add(newCompounds[i].GetName);
            }
        }

        private void updateExistComp()
        {
            comboBoxComp.Items.Clear();
            //for each compound
                for (int i = 0; i < compounds.Count; i++)
                {
                    //add the compound to the menu
                    comboBoxComp.Items.Add(compounds[i].name);
            }
        }

        //create
        private void button1_Click(object sender, EventArgs e)
        {
            //add the compound to the array
            compounds.Add(newCompounds[comboBoxNewComp.SelectedIndex]);

            //for each element required in the compound
            int[] tmpFormula = newCompounds[newCompounds.Count-1].elements;
            for(int i = 0; i < tmpFormula.Length; i++)
            {

                //remove the elements
                deck.List[elements[tmpFormula[i]-1][0].ID].state = 4;
                hand.Remove(elements[tmpFormula[i]-1][0]);
                elements[tmpFormula[i]-1].RemoveAt(0);
            }

            //update
            updateExistComp();
            updateElements();
            updateNewComp();
        }

        //destroy
        private void button2_Click(object sender, EventArgs e)
        {
            //tmp variables
            int[] tmpComp = compounds[comboBoxComp.SelectedIndex].elements;
            List<List<Element>> tempEle = elements;

            //for each element inside the compound
            if (tmpComp.Length >= energy)
            {
                for (int i = 0; i < tmpComp.Length; i++)
                {
                    //add the element to the hand
                    hand.Add(tempEle[tmpComp[i]][0]);
                    deck.List[hand[hand.Count].ID].state = 2;
                    tempEle[tmpComp[i]].RemoveAt(0);
                    //take away energy
                    energy--;
                }

                //remove the compound from the array
                compounds.RemoveAt(comboBoxComp.SelectedIndex);

                //update
                updateElements();
                updateExistComp();
                updateNewComp();
            }
        }

        //update formula text
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxNewComp.Text = newCompounds[comboBoxNewComp.SelectedIndex].formula;
        }

        //update the formula text
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxComp.Text = compounds[comboBoxComp.SelectedIndex].formula;
        }

        //Close the crucible and update the player
        private void buttonFinish_Click(object sender, EventArgs e)
        {

            player.Hand = hand;
            player.Compounds = compounds;
            player.Energy = energy;
            player.Deck = deck;

            Field.craftingDone();
            this.Close();
        }

        //needs to be updated
        public Compound createCompound(String name)
        {
            if (name == "NaHCO₃")
            {
                return new NaHCO3();
            }
            else if (name == "NaClO")
            {
                return new NaClO();
            }
            else if (name == "N₂O")
            {
                return new N2O();
            }
            else if (name == "CaCO₃")
            {
                return new CaCO3();
            }
            else if (name == "C₃H₈O")
            {
                return new C3H8O();
            }
            else if (name == "C₁₂H₂₂O₁₁")
            {
                return new C12H22O11();
            }
            else if (name == "SiO₂")
            {
                return new SiO2();
            }
            else if (name == "HCl")
            {
                return new HCl();
            }
            else if (name == "NaCl")
            {
                return new NaCl();
            }
            else if (name == "NaOH")
            {
                return new NaOH();
            }
            else if (name == "C₈H₉NO₂")
            {
                return new C8H9NO2();
            }
            else if (name == "H₂O")
            {
                return new H2O();
            }
            else if (name == "H₂O₂")
            {
                return new H2O2();
            }
            else if (name == "KOH")
            {
                return new KOH();
            }
            return null;
        }
    }
}
