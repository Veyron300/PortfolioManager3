using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManager3.WPFUtility
{
    public class OnPositionAddedEventArgs
    {
        public ObservableCollection<Position> Positions { get; set; }
        public Position Position { get; set; }
        public IEnumerable<Analytic> AnalyticList { get; set; }
    }
}
