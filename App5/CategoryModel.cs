using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace App5
{
    class CategoryModel
    {
        public String Title { get; set; }
        public String LinkRss { get; set; }

        public ObservableCollection<ItemNewsPaper> NewsItem { get; set; }
        public CategoryModel(){
            NewsItem = new ObservableCollection<ItemNewsPaper>();
        }
    }
    
}
