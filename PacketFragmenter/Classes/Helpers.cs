using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace PacketFragmenter.Classes
{
    internal static class Helpers
    {
        private static int EightMultiple(int number)
        {
            return number - (number % 8);
        }

        public static List<Fragment> FragmentData(Fragment data, int mtu, int header = 20)
        {
            var localData = new Fragment(data);
            var result = new List<Fragment>();
            while (localData.Length + header > mtu)
            {
                var temp = new Fragment(EightMultiple(mtu - header),
                                            true,
                                            localData.Offset);

                localData.Length -= temp.Length;
                localData.Offset = (temp.Length / 8) + ((result.Count > 0) ? result.Last().Offset : 0);
                result.Add(temp);
            }
            if (localData.Length != 0)
            {
                localData.Offset += (result.Count > 0) ? result.Last().Offset : 0;
                result.Add(localData);
            }

            return result;
        }

        static public Table FormatFragmentsList(IEnumerable<Fragment> fragments)
        {
            var thetable = new Table();
            var tableRowGroup = new TableRowGroup();

            var headerRow = new TableRow { Background = new SolidColorBrush(Colors.LightGray) };
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Déplacement"))) { BorderBrush = new SolidColorBrush(Colors.Gray) });
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Taille"))) { BorderBrush = new SolidColorBrush(Colors.Gray) });
            headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Bit More Fragments"))) { BorderBrush = new SolidColorBrush(Colors.Gray) });

            tableRowGroup.Rows.Add(headerRow);

            foreach (var fragment in fragments)
            {
                var row = new TableRow();
                row.Cells.Add(new TableCell(new Paragraph(new Run(fragment.Offset.ToString()))) { BorderBrush = new SolidColorBrush(Colors.Gray), BorderThickness = new Thickness(0, 0, 0, 0.2) });
                row.Cells.Add(new TableCell(new Paragraph(new Run(fragment.Length.ToString()))) { BorderBrush = new SolidColorBrush(Colors.Gray), BorderThickness = new Thickness(0, 0, 0, 0.2) });
                row.Cells.Add(new TableCell(new Paragraph(new Run(fragment.MoreFragments.ToString()))) { BorderBrush = new SolidColorBrush(Colors.Gray), BorderThickness = new Thickness(0, 0, 0, 0.2) });
                tableRowGroup.Rows.Add(row);
            }
            thetable.RowGroups.Add(tableRowGroup);

            return thetable;
        }
    }
}