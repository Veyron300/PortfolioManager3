using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioManager3;
using IMA.API.LocalData;
using System.Collections.ObjectModel;
using PortfolioManager3.InstrumentDataFactory;

namespace PortfolioManager3.WPFUtility
{
    public sealed class Mediator
    {
        /// <summary>
        /// Singleton that facilitates communication between Viewmodels
        /// </summary>
        private static readonly Mediator _instance = new Mediator();

        static Mediator() {}
        
        private Mediator() {}
    
        public static Mediator Instance
        {
            get { return _instance; }
        }

        public event EventHandler<PortfolioChangedEventArgs> PortfolioChanged;

        public event EventHandler<AddInstrumentEventArgs> AddInstrument;

        public event EventHandler<OnPositionAddedEventArgs> AddPosition;

        public void OnPortfolioChanged(object sender, Portfolio portfolio)
        {
            
            var portfolioChangedDelegate = PortfolioChanged as EventHandler<PortfolioChangedEventArgs>;
            if (portfolioChangedDelegate != null)
            {
                portfolioChangedDelegate(sender, new PortfolioChangedEventArgs { Portfolio = portfolio, });
            }
        }

        public void OnInstrumentAdded(object sender, SourceInstrumentMetadata instrument)
        {
            var InstrumentSelectedDelegate = AddInstrument as EventHandler<AddInstrumentEventArgs>;
            if(InstrumentSelectedDelegate != null)
            {
                InstrumentSelectedDelegate(sender, new AddInstrumentEventArgs { Instrument = instrument });
                
            }
        }

        public void OnPositionAdded(object sender, Position position)
        {
            var OnPositionAddedDelegate = AddPosition as EventHandler<OnPositionAddedEventArgs>;
            if (OnPositionAddedDelegate != null)
            {
                OnPositionAddedDelegate(sender, new OnPositionAddedEventArgs { Position = position });
            }
        }

    }
}
