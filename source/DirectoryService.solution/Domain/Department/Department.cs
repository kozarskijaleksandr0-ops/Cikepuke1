using Domain.Department.ValueObjects;
using Domain.Position.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Department
{
    public class Department
    {
        public Department(Id id, Id parentid, Name name, Identifier identifier, DepartmentPath path, Depth depth, EntityLifeTime lifeTime)
        {
            Id = id;
            ParentId = parentid;
            Name = name;
            Depth = depth;
            Identifier = identifier;
            Path = path;
            LifeTime = lifeTime;
        }
        public Id Id { get; }
        public Id ParentId { get; }
        public Name Name { get; }
        public Identifier Identifier { get; }
        public DepartmentPath Path { get; }
        public Depth Depth { get; }
        public EntityLifeTime LifeTime { get; }
    }

}
