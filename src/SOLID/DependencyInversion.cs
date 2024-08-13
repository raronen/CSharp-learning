// Principle: High level parts of the system shouold not depend on low level parts of the system. Both should depend on abstractions.
// היפוך תלות - תלות גבוהה לא תלויה בתלות נמוכה

namespace DesignPatterns {
    public enum Relationship {
        Parent,
        Child,
        Sibling
    }

    public class Person {
        public string Name;
    }

    // Low-level 
    public class Relationships {
        private List<(Person, Relationship, Person)> relations = new List<(Person, Relationship, Person)>();
        
        public void AddParentAndChild(Person parent, Person child) {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }

        public List<(Person, Relationship, Person)> Relations => relations; // Breaks the Dependency Inversion principle - High level module should not depend on low level module.    }
        // Relationships can't change the way it saves the relation -> It can't switch it to dictionary or something else.
        // Solution -> Abstraction!
    } 

    // Solution:
    public interface IRelationshipBrowser {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }

    public class BetterRelationships: IRelationshipBrowser {
        private List<(Person, Relationship, Person)> relations = new List<(Person, Relationship, Person)>();
        
        public void AddParentAndChild(Person parent, Person child) {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }

        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
           foreach (var relation in relations)
           {
             if (relation.Item1.Name == name && relation.Item2 == Relationship.Parent) {
                yield return relation.Item3;
             }
           }
        }
    } 


    // High level
    public class Research {
        // Old way
        public Research(Relationships relationships)
        {
            var relations = relationships.Relations; // Breaks the Dependency Inversion principle - High level module should not depend on low level module.

            foreach (var r in relations) {
                if (r.Item1.Name == "John" && r.Item2 == Relationship.Parent) {
                    Console.WriteLine($"{r.Item1.Name} has a child called {r.Item3.Name}");
                }   
            }
        }

        public Research(BetterRelationships relationships)
        {
            foreach (var p in relationships.FindAllChildrenOf("John")) {
                Console.WriteLine($"John has a child called {p.Name}");
            }
        }
    }
}