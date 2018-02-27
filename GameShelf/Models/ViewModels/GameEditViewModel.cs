﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameShelf.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameShelf.Models.ViewModels
{
    public class GameEditViewModel
    {
        public GameWithPersonInfo GameWithPersonInfo { get; set; }
        public SelectList PlayTimeSelect { get; set; }
        public List<AssignedPersonData> AllPersonsData { get; set; }
        public MultiSelectList OwnerSelect { get; set; }

        public GameEditViewModel(GameShelfContext db, int id)
        {
            GameWithPersonInfo = new GameWithPersonInfo(db, id);

            var playTimeQuery = db.Playtimes.OrderBy(pt => pt.ID);
            PlayTimeSelect = new SelectList(playTimeQuery, "ID", "PlayTimeCategory", GameWithPersonInfo.PlayTimeID);        

            var personQuery = db.People.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);

            AllPersonsData = new List<AssignedPersonData>();
            foreach (Person person in personQuery)
            {
                AllPersonsData.Add(new AssignedPersonData
                {
                    PersonID = person.ID,
                    FullName = person.FullName,
                    AssignedOwner = GameWithPersonInfo.Owners.Contains(person),
                    AssignedDesigner = GameWithPersonInfo.Designers.Contains(person)
                });
            }

            List<SelectListItem> personItems = new List<SelectListItem>();
            foreach (var person in AllPersonsData)
            {
                var personItem = new SelectListItem
                {
                    Value = person.PersonID.ToString(),
                    Text = person.FullName
                };

                personItems.Add(personItem);
            }

            OwnerSelect = new MultiSelectList(personItems.OrderBy(pi => pi.Text), "Value", "Text");
        }
    }
}