using DesignPatterns;

var parent = new Person { Name = "John" };
var child1 = new Person { Name = "Chris" };
var child2 = new Person { Name = "Matt" };

// Old
var relationships = new Relationships();

relationships.AddParentAndChild(parent, child1);
relationships.AddParentAndChild(parent, child2);

new Research(relationships);

// New - Better
var betterRelationships = new BetterRelationships();

betterRelationships.AddParentAndChild(parent, child1);
betterRelationships.AddParentAndChild(parent, child2);

new Research(betterRelationships);