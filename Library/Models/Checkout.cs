using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
    public class Checkout
    {
        private int _id;
        private int _patronId;
        private int _copiesId;
        private DateTime _dueDate;
        private DateTime _checkoutDate;
        private DateTime _returnedDate;
        private bool _returnedStatus;

        public Checkout(int patronId, int copiesId, DateTime dueDate, DateTime checkoutDate, DateTime returnedDate, bool returnedStatus, int id = 0)
        {
            _patronId = patronId;
            _copiesId = copiesId;
            _dueDate = dueDate;
            _checkoutDate = checkoutDate;
            _returnedDate = returnedDate;
            _returnedStatus = returnedStatus;
            _id = id;
        }

        public int GetId()
        {
            return _id;
        }
        public int GetPatronId()
        {
            return _patronId;
        }
        public int GetCopiesId()
        {
            return _copiesId;
        }
        public DateTime GetDueDate()
        {
            return _dueDate;
        }
        public DateTime GetCheckoutDate()
        {
            return _checkoutDate;
        }
        public DateTime GetReturnedDate()
        {
            return _returnedDate;
        }
        public bool GetReturnedStatus()
        {
            return _returnedStatus;
        }

        // public static List<Checkout> GetOverDueBooks()
        // {
        //     return 
        // }


    }
}

