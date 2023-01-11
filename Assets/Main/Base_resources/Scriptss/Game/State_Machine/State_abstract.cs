
    public abstract class State_abstract
    {
        protected Game_character_abstract Character_script;
        protected StateMachine State_Machine_script;

        bool Start_preparation_bool = false;//Подготовка

        protected State_abstract(Game_character_abstract character, StateMachine stateMachine)
        {
            Character_script = character;
            State_Machine_script = stateMachine;
        }

    /// <summary>
    /// Вход состояния (начало реализации)
    /// </summary>
        public virtual void Enter_state()
        {
           if (!Start_preparation_bool)
           {
              Start_preparation_bool = true;
              Preparation();
           }
        }

    /// <summary>
    /// Начальная подготовка
    /// </summary>
    protected virtual void Preparation()
    {

    }

    /// <summary>
    /// Блок относящийся к задаванию управления (в основном от игрока)
    /// </summary>
        public virtual void Handle_Input()
        {

        }


    /// <summary>
    /// Логика с большей задержкой, чем Update (так сказать для снижения нагрузки)
    /// </summary>
    public virtual void Slow_Update()
    {

    }

    /// <summary>
    /// Логика с большей задержкой, чем Update (второй) (так сказать для снижения нагрузки)
    /// </summary>
    public virtual void Slow_Update_2()
    {

    }


    /// <summary>
    /// Логика поведения этого состояния
    /// </summary>
    public virtual void Logic_Update()
    {

    }

    /// <summary>
    /// Физика и методы которые к ней относятся (например передвижение)
    /// </summary>
        public virtual void Physics_Update()
        {

        }


    /// <summary>
    /// Выход из состояния
    /// </summary>
        public virtual void Exit_state()
        {

        }
    }
