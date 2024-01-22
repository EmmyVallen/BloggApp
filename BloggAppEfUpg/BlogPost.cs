using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggAppEfUpg
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        /*
         "List" är en inbyggd generisk klass i C# som representerar en starkt skriven 
        samling objekt. I det här fallet är typen av objekt i samlingen "Kategori", 
        vilket är en annan klass som representerar en kategori som är associerad med ett blogginlägg.
        Egenskapen "Kategorier" är alltså en lista över kategoriobjekt som kan nås via alla 
        instanser av klassen som innehåller den här egenskapen. Det innebär att du 
        för alla instanser av klassen kan komma åt
        egenskapen "Kategorier" för att få en lista över alla kategorier som är associerade med den instansen
         */
        public List<Category> Categories { get; set; } = new List<Category>();

        /*
         Gränssnittet "ICollection" är ett inbyggt gränssnitt i C# som definierar en samling objekt. 
        Den innehåller metoder för att lägga till, ta bort och komma åt element i samlingen.
        Egenskapen "Taggar" är alltså en samling taggobjekt som kan nås via valfri instans av klassen som innehåller den här egenskapen. 
        Det innebär att du för alla instanser av klassen kan komma åt egenskapen "Taggar" för att få en samling av alla taggar som är associerade med den instansen.
         */
        public ICollection<Tag> Tags { get; set; }
    }
}
