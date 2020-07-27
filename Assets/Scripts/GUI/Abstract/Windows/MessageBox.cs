using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : WindowBase {

    public TMPro.TextMeshProUGUI head;
    public TMPro.TextMeshProUGUI body;

    public Button first;
    public Button second;
    public Button third;

    private DialogResult result = DialogResult.None;

    private string text;
    private string caption;

    //private Window blackBlank;
    private WindowBase btn0;
    private WindowBase btn2;
    private WindowBase btn1;

    private UIManager instanceUI;

    
	protected override void OnEnable() {
        base.OnEnable();

        btn0 = first.GetComponent<WindowBase>();
        btn1 = second.GetComponent<WindowBase>();
        btn2 = third.GetComponent<WindowBase>();

       
    }
	

    public DialogResult GetResult() {
        return result;
    }

    public void Show( string text, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None) {
        head.text = caption;
        body.text = text;

        DetermineType(buttons);

        this.Show();
    }
    public void ShowDialoge( string text, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None ) {
        head.text = caption;
        body.text = text;

        DetermineType(buttons);

        UIManager._instance.BlackBlank.Show();
        this.ShowDialoge();
    }

    public override void Close() {
        base.Close();

        UIManager._instance.BlackBlank.Close();
    }
    private void Dispose() {
        UIManager._instance.DisposeWindow(this);
    }

    /// <summary>
    /// Какие кнопки должны быть на форме и смена DialogResult
    /// </summary>
    /// <param name="buttons"></param>
    private void DetermineType( MessageBoxButtons buttons) {

		switch (buttons) {
            case MessageBoxButtons.AbortRetryIgnore: {

			}break;
            case MessageBoxButtons.OK: {

                second.onClick.AddListener(delegate { result = DialogResult.OK; });

                second.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "OK";

                btn1.Show();
            }
            break;
            case MessageBoxButtons.OKCancel: {

                first.onClick.AddListener(delegate { result = DialogResult.OK; });
                third.onClick.AddListener(delegate { result = DialogResult.Cancel; });

                first.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "OK";
                third.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "CANCEL";



                btn0.Show();
                btn2.Show();
            }
            break;
            case MessageBoxButtons.RetryCancel: {

            }
            break;
            case MessageBoxButtons.YesNo: {

                first.onClick.AddListener(delegate { result = DialogResult.Yes; });
                third.onClick.AddListener(delegate { result = DialogResult.No; });

                first.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "YES";
                third.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "NO";



                btn0.Show();
                btn2.Show();
            }
            break;
            case MessageBoxButtons.YesNoCancel: {
                first.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "YES";
                second.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "NO";
                third.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "CANCEL";


                btn0.Show();
                btn1.Show();
                btn2.Show();
            }
            break;
        }
        //после выбора ответа надо закрыть форму
        first.onClick.AddListener(delegate { Close(); Dispose(); });
        second.onClick.AddListener(delegate { Close(); Dispose(); });
        third.onClick.AddListener(delegate { Close(); Dispose(); });

    }
}
/// <summary>
/// Результат, если None то пользователь не выбрал операцию.
/// </summary>
public enum DialogResult : int {
    /// <summary>
    /// Отсутствие результата
    /// </summary>
    None = -1,

    /// <summary>
    /// Нажата кнопка OK
    /// </summary>
    OK = 1,
    /// <summary>
    /// Нажата кнопка Cancel
    /// </summary>
    Cancel = 2,
    /// <summary>
    /// нажата кнопка Yes
    /// </summary>
    Yes = 3,
    /// <summary>
    /// нажата кнопка No
    /// </summary>
    No = 4,
    /// <summary>
    /// Нажата кнопка Abort
    /// </summary>
    Abort = 0
}

/// <summary>
/// Какие кнопки должны быть на форме.
/// </summary>
public enum MessageBoxButtons : int {
    AbortRetryIgnore = 0,//три кнопки Abort (Отмена), Retry( Повтор), Ignore( Пропустить)
    /// <summary>
    /// Одна кнопка OK.
    /// </summary>
    OK = 1,
    /// <summary>
    /// Две кнопки OK и Cancel( Отмена).
    /// </summary>
    OKCancel = 2,
    RetryCancel = 3,//две кнопки Retry( Повтор) и Cancel( Отмена)
    /// <summary>
    /// Две кнопки Yes и No.
    /// </summary>
    YesNo = 4,
    YesNoCancel = 5//три кнопки Yes, No и Cancel (Отмена)
}
public enum MessageBoxIcon : int{
    Asterisk = 0,//Information: значок, состоящий из буквы i в нижнем регистре, помещенной в кружок
    Error = 1, Hand = 2, Stop = 3,//значок, состоящий из белого знака "X" на круге красного цвета.
    Exclamation = 4, Warning = 5,//значок, состоящий из восклицательного знака в желтом треугольнике
    Question = 6,//значок, состоящий из вопросительного знака на периметре круга
    None = 7//значок у сообщения отсутствует
}
