﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coevolution.Models
{
    public class DtoConverter
    {
        public static DtoItem ConvertToDto(Item item)
        {
            if (item is Node)
            {
                return ConvertToDto((Node)item);
            }
            else if (item is Leaf)
            {
                return ConvertToDto((Leaf)item);
            }
            else
            {
                throw new ArgumentException("ConvertToDto only knows how to convert Node or Leaf");
            }
        }

        public static DtoNode ConvertToDto(Node node)
        {
            return new DtoNode()
            {
                Id = node.Id,
                Key = node.Key,
                Parent = node.Parent == null ? (int?) null : node.Parent.Id,
                Date = node.Date,
                Deleted = node.Deleted,
                Labels = node.Labels.Select(label => label.Content).ToList(),
                Notes = node.Notes.Select(note => note.Content).ToList(),
                Children = node.Children.Select<Item, int>(n => n.Id).ToList()
            };
        }

        public static DtoLeaf ConvertToDto(Leaf leaf)
        {
            if (leaf.Parent == null)
            {
                throw new NullReferenceException("Leaf's parent cannot be null.");
            }
            return new DtoLeaf()
            {
                Id = leaf.Id,
                Key = leaf.Key,
                Parent = leaf.Parent.Id,
                Date = leaf.Date,
                Deleted = leaf.Deleted,
                Labels = leaf.Labels.Select(label => label.Content).ToList(),
                Notes = leaf.Notes.Select(note => note.Content).ToList(),
                Value = leaf.Value
            };
        }

        public static Item ConvertToDomainObject(DtoItem dtoItem, Node parent)
        {
            if (dtoItem is DtoNode)
            {
                return  ConvertToDomainObject((DtoNode) dtoItem, parent);
            }
            else if (dtoItem is DtoLeaf)
            {
                return ConvertToDomainObject((DtoLeaf)dtoItem, parent);
            }
            else
            {
                throw new ArgumentException("ConvertToDomainObject only knows how to convert to Node or Leaf");
            }
        }

        public static Node ConvertToDomainObject(DtoNode dtoNode, Node parent)
        {
            return new Node()
            {
                Key = dtoNode.Key,
                Parent = parent,
                Date = dtoNode.Date,
                Deleted = dtoNode.Deleted,
            };
        }

        public static Leaf ConvertToDomainObject(DtoLeaf dtoLeaf, Node parent)
        {
            return new Leaf()
            {
                Key = dtoLeaf.Key,
                Parent = parent,
                Date = dtoLeaf.Date,
                Deleted = dtoLeaf.Deleted,
                Value = dtoLeaf.Value
            };
        }

    }
}