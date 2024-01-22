using Azure;
using BloggAppEfUpg;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggAppEfUpg
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new BlogDbContext())
            {
                /*
                Om användaren anger ett giltigt kategorinamn hämtar den den befintliga kategorin från en 
                databas med hjälp av Entity Framework. 
                Om användaren anger en ny kategori skapas en ny kategori med indatanamnet och läggs till i databasen.

                Sedan uppmanas användaren att ange en titel och text för blogginlägget. Det skapar ett nytt 
                BlogPost-objekt med inmatningstiteln och texten, 
                lägger till den tidigare hämtade eller skapade kategorin i inlägget och sparar den i databasen.
                 */
                while (true)
                {

                    Console.Write("Enter category (or q to show the menu): ");

                    var categoryInput = Console.ReadLine();
                    if (categoryInput == "q")
                        break;


                    Category category;
                    if (context.Categories.Any(c => c.Name == categoryInput))
                    {
                        category = context.Categories.Single(c => c.Name == categoryInput);
                    }
                    else
                    {
                        category = new Category { Name = categoryInput };
                        context.Categories.Add(category);
                        context.SaveChanges();
                    }

                    Console.Write("Enter blog title: ");

                    var title = Console.ReadLine();


                    Console.Write("Enter blog text: ");

                    var text = Console.ReadLine();

                    Console.Write("Enter a tag to tag the blogpost with (or q to skip): ");
                    var tagInput = Console.ReadLine();
                    List<Tag> tags = new List<Tag>();
                    while(tagInput != "q")
                    {
                        var tag = new Tag { Name = tagInput };
                        tags.Add(tag);
                        Console.Write("Enter another tag to tag the blogpost with (or q to skip): ");
                        tagInput = Console.ReadLine();
                    }
                    var blogPost = new BlogPost { Title = title, Text = text, Categories = new List<Category> { category }, Tags = tags };
                    context.BlogPosts.Add(blogPost);
                    context.SaveChanges();
                    Console.WriteLine();
                    Console.WriteLine("Blogpost added!");
                    Console.WriteLine();
                }

                while (true)

                {
                    Console.WriteLine();
                    Console.WriteLine("Menu");
                    Console.WriteLine("1. Display all posts and end the program");
                    Console.WriteLine("2. Display the names of all the categories");
                    Console.WriteLine("3. Add new blog post");
                    Console.WriteLine("4. Add a new category");
                    Console.WriteLine("5. Display all the blog titles from a category");
                    Console.WriteLine("6. Add an existing blog post to an existing category");
                    Console.WriteLine("7. Create a tag");
                    Console.WriteLine("8. Add existing blog post to an existing tag");
                    Console.WriteLine("9. Display all the blog posts that are tagged with a tag");
                    Console.Write("Select an option: ");


                    var userInput = Console.ReadLine();
                    /*
                    För varje kategori filtreras de blogginlägg som är associerade med den kategorin med hjälp av LINQ-metoden Var. 
                    Den kontrollerar om id-egenskapen för varje blogginlägg är lika med id-egenskapen för den aktuella kategorin.
                    Utdata visar kategorierna och blogginläggen som är associerade med varje kategori.
                     */
                    if (userInput == "1")
                    {
                        // Display all posts
                        var blogPosts = context.BlogPosts.ToList();
                        var categories = context.Categories.ToList();
                        foreach (var category in categories)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Categories: {category.Name}");


                            var categoryPosts = blogPosts.Where(post => post.Id == category.Id);

                            foreach (var blogPost in categoryPosts)
                            {

                                Console.WriteLine($"Title: {blogPost.Title}");
                                Console.WriteLine($"Text: {blogPost.Text}");


                            }

                        }
                        break;
                    }
                    else if (userInput == "2")
                    {
                        /*
                         Med hjälp av metoden ToList, kör frågan och returnerar resultatet som en lista över kategoriobjekt.
                         */
                        var categories = context.Categories.ToList();
                        foreach (var category in categories)
                        {
                            Console.WriteLine();
                            Console.WriteLine(category.Name);
                        }
                    }

                    /*
                    Användaren uppmanas att ange kategorinamnet, sedan uppmanas användaren att ange bloggrubriken och texten.
                    Den hämtar kategoriobjektet från databaskontexten med metoden SingleOrDefault, 
                    som returnerar ett enskilt kategoriobjekt som matchar det angivna villkoret eller null om 
                    inget sådant objekt hittas. 
                    I det här fallet hämtar den kategorin med det angivna namnet som användaren har angett.
                    Om ingen kategori med det angivna namnet hittas skapas ett nytt kategoriobjekt med det 
                    angivna namnet och läggs till i databaskontexten 
                     */
                    else if (userInput == "3")
                    {   //Add category
                        Console.WriteLine();
                        Console.Write("Enter category name: ");
                        var categoryName = Console.ReadLine();
                        

                        // Add new blog post
                        Console.WriteLine();
                        Console.Write("Enter blog title: ");
                        var title = Console.ReadLine();

                        Console.Write("Enter blog text: ");
                        var text = Console.ReadLine();

                        var category = context.Categories.SingleOrDefault(c => c.Name == categoryName);
                        if (category == null)
                        {
                            category = new Category { Name = categoryName };
                            context.Categories.Add(category);
                            context.SaveChanges();
                        }

                        var blogPost = new BlogPost { Title = title, Text = text };
                        blogPost.Categories.Add(category);
                        context.BlogPosts.Add(blogPost);
                        context.SaveChanges();

                        Console.WriteLine("Blog post added successfully.");
                    }

                    /*
                    Koden uppmanar användaren att ange namnet på den kategori de vill lägga till och hämtar sedan kategorin 
                    med det namnet från databasen med hjälp av 
                    'SingleOrDefault och sparas i databasen med hjälp av context.SaveChanges().
                    Om kategorin redan finns informerar koden användaren om att kategorin redan finns och skapar inte en ny.
                     */

                    else if (userInput == "4")
                    {
                        Console.WriteLine();
                        Console.Write("Enter category name: ");

                        var categoryName = Console.ReadLine();

                        var category = context.Categories.SingleOrDefault(c => c.Name == categoryName);
                        if (category == null)
                        {
                            var category1 = new Category { Name = categoryName };
                            context.Categories.Add(category1);
                            context.SaveChanges();


                            Console.WriteLine();
                            Console.WriteLine($"Category '{categoryName}' added successfully.");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine($"Category '{categoryName}' already exists.");
                           
                            Console.WriteLine();
                        }
                    }

                    /*
                    Om kategorin hittas används Include metod för att inkludera relaterade BlogPosts objekt i frågeresultatet. 
                    Detta innebär att category.BlogPosts egenskapen fylls i med alla blogginlägg som är associerade med kategorin.
                    Om kategorin inte hittas visar koden helt enkelt ett felmeddelande som indikerar att kategorin inte finns.
                     */
                    else if (userInput == "5")
                    {
                        Console.WriteLine();
                        Console.Write("Enter category name: ");

                        var categoryName = Console.ReadLine();

                        var category = context.Categories
                            .Include(c => c.BlogPosts)
                            .SingleOrDefault(c => c.Name == categoryName);

                        if (category != null)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Blog posts in category '{categoryName}':");
                            Console.WriteLine();

                            foreach (var post in category.BlogPosts)
                            {
                                Console.WriteLine(post.Title);
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Category '{categoryName}' does not exist.");
                            
                        }
                    }
                    /*
                    Koden söker efter kategorin i databasen, och om den inte finns informerar den användaren.
                    Om kategorin finns söker koden efter blogginlägget i databasen, och om den inte finns informerar den användaren.
                    Om både kategorin och blogginlägget finns kontrollerar koden om blogginlägget redan är associerat med kategorin. 
                    Om det är så informerar det användaren.
                    Om blogginlägget inte är associerat med kategorin lägger koden till kategorin i blogginläggets kategorisamling, 
                    sparar ändringarna i databasen och informerar användaren om att blogginlägget har lagts till i kategorin.
                     */
                    else if (userInput == "6")
                    {
                        Console.Write("Enter category name: ");
                        var categoryName = Console.ReadLine();

                        Console.Write("Enter blog post: ");
                        var postId = (Console.ReadLine());

                        var category = context.Categories.SingleOrDefault(c => c.Name == categoryName);

                        if (category == null)
                        {
                            Console.WriteLine($"Category '{categoryName}' does not exist.");
                            return;
                        }
                        else
                        {
                            var post = context.BlogPosts.SingleOrDefault(p => p.Title == postId);

                            if (post == null)
                            {
                                Console.WriteLine($"Blog post with ID {postId} does not exist.");
                                return;
                            }
                            else
                            {
                                if (post.Categories.Any(c => c.Name == categoryName))
                                {
                                    Console.WriteLine($"Blog post '{post.Title}' already belongs to category '{categoryName}'.");
                                    return;
                                }
                                else
                                {
                                    post.Categories.Add(category);
                                    context.SaveChanges();

                                    Console.WriteLine($"Blog post '{post.Title}' added to category '{categoryName}'.");
                                }

                            }
                        }
                    }

                    /*
                    Koden kontrollerar sedan om det finns en befintlig tagg i databasen med samma namn med 
                    hjälp av metoden 'FirstOrDefault'.
                    Om en befintlig tagg hittas informerar koden användaren om att taggen redan finns. 
                    Om en befintlig tagg inte hittas skapas en ny tagg med det namn som användaren har angett. 
                    Den nya taggen läggs sedan till i databasen
                    */
                    else if (userInput == "7")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Enter the name of the tag:");
                        string name = Console.ReadLine();

                        var existingTag = context.Tags.FirstOrDefault(t => t.Name == name);

                        if (existingTag != null)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Tag '{name}' already exists.");
                           
                        }
                        else
                        {
                            var tag = new Tag { Name = name };

                            context.Tags.Add(tag);
                            context.SaveChanges();
                            Console.WriteLine();
                            Console.WriteLine($"Tag '{name}' created.");
                        }
                    }


                    /*
                    Koden hämtar blogginlägget med matchande titel från databasen och
                    kontrollerar om det finns. Om det inte finns skriver det ut ett  meddelande 
                    till konsolen som säger att blogginlägget inte hittades.
                    Om blogginlägget finns hämtar koden taggen med matchande namn från databasen och kontrollerar om den finns. 
                    Om den inte finns skriver den ut ett meddelande till konsolen om att taggen inte hittades.
                    Om både blogginlägget och taggen finns kontrollerar koden om blogginläggets taggsamling är null. 
                    Om det är null initierar koden samlingen som en ny tom lista.
Taggen läggs till i blogginläggets taggsamling.
                    */
                    else if (userInput == "8")
                    {
                        Console.WriteLine("Enter the title of the blog post:");
                        var blogPostId = Console.ReadLine();

                        Console.WriteLine("Enter the name of the tag:");
                        var tagId = Console.ReadLine();

                        var blogPost = context.BlogPosts.SingleOrDefault(b => b.Title == blogPostId);

                        if (blogPost == null)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Blog post with name '{blogPostId}' not found.");
                        }
                        else
                        {
                            var tag = context.Tags.SingleOrDefault(t => t.Name == tagId);

                            if (tag == null)
                            {
                                Console.WriteLine();
                                Console.WriteLine($"Tag with name '{tagId}' not found.");
                            }
                            else
                            {
                                blogPost.Tags = blogPost.Tags ?? new List<Tag>(); // initialize blogPost.Tags if null
                                blogPost.Tags.Add(tag);
                                context.SaveChanges();
                                Console.WriteLine();
                                Console.WriteLine($"Blog post '{blogPost.Title}' added to tag '{tag.Name}'.");
                            }
                        }
                    }



                    /*
                    koden letar efter den första taggen i databasen som har samma namn som den som användaren angav.
                    Om ingen tagg hittas informerar programmet användaren och avslutar blocket.
                    Om en tagg hittas hämtar programmet en lista över alla blogginlägg som har den taggen tilldelad.
                    Om inga blogginlägg hittas med taggen informerar programmet användaren och avslutar blocket.
                    Om blogginlägg hittas med taggen visar programmet titeln på varje blogginlägg.
                    */
                    else if (userInput == "9")
                    {
                        Console.WriteLine("Enter tag:");
                        string tagName = Console.ReadLine();

                        Tag tag = context.Tags.FirstOrDefault(t => t.Name == tagName);
                        if (tag == null)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Tag not found.");
                           
                        }
                        else
                        {
                            // Get blog posts with tag
                            List<BlogPost> blogPosts = tag.BlogPosts.ToList();
                            if (blogPosts == null)
                            {
                                Console.WriteLine();
                                Console.WriteLine($"No blog posts found with tag:{tag.Name}");
                                
                            }
                            else
                            {
                                // Display blog post titles
                                Console.WriteLine($"Blog posts with tag: {tag.Name}");
                                foreach (BlogPost blogPost in blogPosts)
                                {

                                    Console.WriteLine($"- {blogPost.Title}");
                                }
                            }
                        }
                    }

                }
            }
        }

    }

}
