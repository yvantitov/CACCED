using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Battle
{
    public int turn = 1;

    public int pLeft;
    public int pCenter;
    public int pRight;

    public int eLeft;
    public int eCenter;
    public int eRight;
    public bool eLeftIntel = false;
    public bool eCenterIntel = false;
    public bool eRightIntel = false;

    public bool pLeftRouted = false;
    public bool pCenterRouted = false;
    public bool pRightRouted = false;

    public bool eLeftRouted = false;
    public bool eCenterRouted = false;
    public bool eRightRouted = false;

    public Battle(
        int pLeft,
        int pCenter,
        int pRight,
        int eLeft,
        int eCenter,
        int eRight,
        bool eLeftIntel,
        bool eCenterIntel,
        bool eRightIntel)
    {
        this.pLeft = pLeft;
        this.pCenter = pCenter;
        this.pRight = pRight;
        this.eLeft = eLeft;
        this.eCenter = eCenter;
        this.eRight = eRight;
        this.eLeftIntel = eLeftIntel;
        this.eCenterIntel = eCenterIntel;
        this.eRightIntel = eRightIntel;
    }
}
