//======================================================================
//
//        Copyright (C) 2020-2021 个人软件工作室    
//        All rights reserved
//
//        filename :TextLabel.cs
//        description :
//
//        created by 张恭亮 at  2020/10/14 20:04:37
//
//======================================================================

using FastAutomationFrame.Diagram;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicsShape
{
	public class TextLabel : ShapeBase
	{
		#region Fields


		#endregion

		#region Proterties

		[Browsable(false)]
		public override string ShapeName => "TextLabel";

		#endregion

		#region Constructor

		/// <summary>
		/// Default ctor
		/// </summary>
		/// <param name="s"></param>
		public TextLabel(DiagramControl s) : base(s)
		{
			this.backGroundColor = Color.Transparent;
		}

		public TextLabel() : base()
		{
			this.backGroundColor = Color.Transparent;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Paints the shape on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{
			if (this.HideItem) return;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			if (Image != null)
			{
				g.DrawImage(Image,
					rectangle.X + this.site.ViewOriginPoint.GetPoint().X,
					rectangle.Y + this.site.ViewOriginPoint.GetPoint().Y,
						rectangle.Width,
						rectangle.Height); //在窗口的画布中绘画出内存中的图像
			}
			else
			{
				Brush brush = new SolidBrush(BackGroundColor);
				g.FillRectangle(brush,
						rectangle.X + this.site.ViewOriginPoint.GetPoint().X,
						rectangle.Y + this.site.ViewOriginPoint.GetPoint().Y,
						rectangle.Width,
						rectangle.Height);
			}

			if (hovered || isSelected)
			{
				Pen p = new Pen(BorderSelectedColor, 2F);
				p.StartCap = LineCap.Round;
				p.EndCap = LineCap.Round;
				p.LineJoin = LineJoin.Round;
				g.DrawRectangle(p,
					rectangle.X + this.site.ViewOriginPoint.GetPoint().X,
					rectangle.Y + this.site.ViewOriginPoint.GetPoint().Y,
					rectangle.Width,
					rectangle.Height);
			}
			else if (ShowBorder)
			{
				Pen p = new Pen(BorderColor);
				p.StartCap = LineCap.Round;
				p.EndCap = LineCap.Round;
				p.LineJoin = LineJoin.Round;
				g.DrawRectangle(p,
					rectangle.X + this.site.ViewOriginPoint.GetPoint().X,
					rectangle.Y + this.site.ViewOriginPoint.GetPoint().Y,
					rectangle.Width,
					rectangle.Height);
			}

			if (text != string.Empty)
			{
				StringFormat stringFormat = new StringFormat();
				stringFormat.LineAlignment = StringAlignment.Center;
				stringFormat.Alignment = StringAlignment.Center;
				g.DrawString(text, font, Brushes.Black, rectangle, stringFormat);
			}
		}

		#endregion
	}
}
