using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Model
{


    public abstract class TaskRunner : MonoBehaviour
    {

        public static TaskRunner Instance;

        async void Awake()
        {

            //
            await new WaitForSec(3);


            //
        }

       public  List<UintyTask> all = new List<UintyTask>();


        void Update()
        {
            //for (int i = 0; i < length; i++)
            //{



            //}
            foreach (var item in all)
            {
                item.OnRuningFrame();
            }

        }
    }


    public abstract class UintyTask : INotifyCompletion
    {
        public UintyTask()
        {
            TaskRunner.Instance.all.Add(this);
        }
        public abstract bool OnRuningFrame();
        Action? Action;
        public bool IsCompleted { get; private set; } = false;
        public  bool Alive => !IsCompleted;
        public void OnCompleted(Action continuation)
        {
            Action = continuation;
        }
        public void GetResult()
        {
        }
        public UintyTask GetAwaiter()
        {
            return this;
        }
        internal  void Update()
        {
            try
            {
                if (!IsCompleted && OnRuningFrame())
                {
                    End();
                }
            }
            catch (Exception ex)
            {
                End();
            }
        }
        void End()
        {
            try
            {
                if (!IsCompleted)
                {
                    IsCompleted = true;
                    Action?.Invoke();
                    TaskRunner.Instance.all.Remove(this);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }



    public class WaitForSec : UintyTask
    {
        double sec;
        public WaitForSec(int sec)
        {
            Task task;
           
            this.sec = sec;
        }
        public override bool OnRuningFrame()
        {
            if ((this.sec -= Time.deltaTime )<= 0)
            {
                return true;
            }
            return false;


        }
    }

}
