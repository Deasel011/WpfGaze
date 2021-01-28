using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using Stockgaze.Core.Manager;
using Stockgaze.Core.Models;
using Stockgaze.Core.Scheduling;
using Stockgaze.Core.Services;
using Stockgaze.Core.WPFTools;

namespace Stockgaze.Core.Synchronization
{

    public class SynchronizationUCVM : BindableBase
    {

        private bool m_isRefreshing;

        private QuestradeOptionManager m_optionManager;

        private QuestradeSymbolDataManager m_symbolDataManager;

        private QuestradeSymbolIdManager m_symbolIdManager;

        private SchedulingManager m_schedulingManager;

        private EmailService m_emailService;

        private string m_smtpServer;

        private string m_from;

        private string m_to;

        private string m_password;

        private string m_username;

        private string m_port;

        private string m_protocol;

        public string SmtpServer
        {
            get => m_smtpServer;
            set 
            {
                if (SetProperty(ref m_smtpServer, value)) 
                    m_emailService.SmtpServer = value;
            }
        }

        public string From
        {
            get => m_from;
            set 
            {
                if (SetProperty(ref m_from, value)) 
                    m_emailService.From = value;
            }
        }

        public string To
        {
            get => m_to;
            set 
            {
                if (SetProperty(ref m_to, value)) 
                    m_emailService.To = value;
            }
        }

        public string Password
        {
            get => m_password;
            set 
            {
                if (SetProperty(ref m_password, value)) 
                    m_emailService.Password = value;
            }
        }

        public string Username
        {
            get => m_username;
            set 
            {
                if (SetProperty(ref m_username, value)) 
                    m_emailService.Username = value;
            }
        }

        public string Port
        {
            get => m_port;
            set 
            {
                if (SetProperty(ref m_port, value)) 
                    m_emailService.Port = value;
            }
        }

        public string Protocol
        {
            get => m_protocol;
            set
            {
                if (SetProperty(ref m_protocol, value)) 
                    m_emailService.Protocol = value;
            }
        }

        public void SaveCurrentEmailConfig()
        {
            m_emailService.EmailConfigFile.Save();
        }

        public bool IsRefreshing
        {
            get => m_isRefreshing;
            set => SetProperty(ref m_isRefreshing, value);
        }

        public QuestradeSymbolIdManager SymbolIdManager
        {
            get => m_symbolIdManager;
            set => SetProperty(ref m_symbolIdManager, value);
        }

        public QuestradeSymbolDataManager SymbolDataManager
        {
            get => m_symbolDataManager;
            set => SetProperty(ref m_symbolDataManager, value);
        }

        public SchedulingManager SchedulingManager
        {
            get => m_schedulingManager;
            set => SetProperty(ref m_schedulingManager, value);
        }

        public QuestradeOptionManager OptionManager
        {
            get => m_optionManager;
            set => SetProperty(ref m_optionManager, value);
        }

        public BindableCollection<InteractiveSchedule> Schedules { get; set; }

        public SynchronizationUCVM(QuestradeSymbolDataManager symbolDataManager, QuestradeSymbolIdManager symbolIdManager, QuestradeOptionManager optionManager, SchedulingManager schedulingManager, EmailService emailService)
        {
            m_symbolDataManager = symbolDataManager;
            m_symbolIdManager = symbolIdManager;
            m_schedulingManager = schedulingManager;
            m_emailService = emailService;
            m_optionManager = optionManager;
            Schedules = new BindableCollection<InteractiveSchedule>(schedulingManager.GetSchedules().Select(s=>new InteractiveSchedule(s)));
        }

        public void SaveSchedule(InteractiveSchedule schedule)
        {
            m_schedulingManager.Schedule(schedule.GetSchedule());
            Schedules.Add(schedule);
        }

        public void SendTestEmail() { m_emailService.SendOptionResults("Options Report",String.Empty, To == null ? new List<string>() :new List<string>(To.Split(';').ToList()), new List<SymbolOptionModel>()); }

    }

}