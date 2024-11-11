using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using UnityEngine;

namespace Core.Data
{
    public class EdgeColorDataProvider: IStaticDataProvider
    {
        public List<EdgeColor> colors;

        private List<EdgeColor> _tempColors;

        public EdgeColorDataProvider(List<EdgeColor> colors)
        {
            this.colors = colors;

            _tempColors = new List<EdgeColor>(3);
        }

        public EdgeColor GetAnotherRandomColor(EdgeColor[] currentColor)
        {
            _tempColors.Clear();
            _tempColors.AddRange(colors);

            for (int i = 0; i < currentColor.Length; i++)
            {
                _tempColors.Remove(currentColor[i]);
            }

            return _tempColors[Random.Range(0, _tempColors.Count)];
        }
    }
}