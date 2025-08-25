
class User
{
    String? Name { get; set; }
    Role Role { get; set; }
}


enum Role
{
    Visitor = 0,
    Parent = 1,
    Student = 2,
    Teacher = 3,
}