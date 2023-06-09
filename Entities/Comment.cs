﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Writing.Payloads.DTOs;

namespace Writing.Entities; 

[Table("Comments_tbl")]
public class Comment : BaseEntity {

    public string content { get; set; }
    
    public Post Post { get; set; }
    
    [MaybeNull]
    public User User { get; set; }
    
}