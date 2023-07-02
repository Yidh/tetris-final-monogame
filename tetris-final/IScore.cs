using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris_final
{
    public interface IScore
    {
        void AddScore(int score);
        List<int> GetTopScores(int count);
    }

}
