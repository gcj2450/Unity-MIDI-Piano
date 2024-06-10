using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
public class ChangeUIEventArgs : EventArgs
{
	private int id;
	private System.Object data;
	private string name;
	private Enum enumST;
	private Dictionary<string, System.Object> attachment;
	private bool boolean;
	private Delegate sendDelegate;
	private ChangeUIEventCallBack eventCallBack;
	private UIEventHandler eventHandler;

	public new static readonly ChangeUIEventArgs Empty = new ChangeUIEventArgs ();
    public delegate void ChangeUIEventCallBack(int callBackCMD, object sender, ChangeUIEventArgs args);
    public delegate void UIEventHandler(object sender, ChangeUIEventArgs args);
    public ChangeUIEventArgs (int id, System.Object data, string name)
	{
		this.id = id;
		this.data = data;
		this.name = name;
	}
    public ChangeUIEventArgs(string _name)
    {
        this.name = _name;
    }
    public ChangeUIEventArgs(string _name,System.Object _data)
    {
        this.Name = _name;
        this.data = _data;
    }

	public ChangeUIEventArgs (int id)
	{
		this.id = id;
	}

	public ChangeUIEventArgs (object data)
	{
		this.data = data;
	}

	public ChangeUIEventArgs ()
	{
		
	}


	public ChangeUIEventArgs (int id, System.Object data)
	{
		this.id = id;
		this.data = data;
	}

	public object Data {
		get { return this.data; }
		set { data = value; }
	}

	public int Id {
		get { return this.id; }
		set { id = value; }
	}

	public string Name {
		get { return this.name; }
		set { name = value; }
	}


	public Enum EnumST {
		get { return this.enumST; }
		set { enumST = value; }
	}


	public Dictionary<string, object> Attachment {
		get { return this.attachment; }
		set { attachment = value; }
	}


	public bool Boolean {
		get { return this.boolean; }
		set { boolean = value; }
	}


	public Delegate SendDelegate {
		get { return this.sendDelegate; }
		set { sendDelegate = value; }
	}

	public ChangeUIEventCallBack EventCallBack
    {
        get { return this.eventCallBack; }
		set { eventCallBack = value; }
    }

	public UIEventHandler EventHandler
    {
		get { return this.eventHandler; }
		set { eventHandler = value; }
    }
}
