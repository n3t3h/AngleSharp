﻿namespace AngleSharp.DOM.Css
{
    using AngleSharp.Css;
    using AngleSharp.Extensions;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// More information available:
    /// https://developer.mozilla.org/en-US/docs/Web/CSS/outline
    /// </summary>
    sealed class CSSOutlineProperty : CSSShorthandProperty, ICssOutlineProperty
    {
        #region Fields

        readonly CSSOutlineStyleProperty _style;
        readonly CSSOutlineWidthProperty _width;
        readonly CSSOutlineColorProperty _color;

        #endregion

        #region ctor

        internal CSSOutlineProperty(CSSStyleDeclaration rule)
            : base(PropertyNames.Outline, rule, PropertyFlags.Animatable)
        {
            _style = Get<CSSOutlineStyleProperty>();
            _width = Get<CSSOutlineWidthProperty>();
            _color = Get<CSSOutlineColorProperty>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the selected outline style property.
        /// </summary>
        public LineStyle Style
        {
            get { return _style.Style; }
        }

        /// <summary>
        /// Gets the selected outline width property.
        /// </summary>
        public Length Width
        {
            get { return _width.Width; }
        }

        /// <summary>
        /// Gets the selected outline color property.
        /// </summary>
        public Color Color
        {
            get { return _color.Color; }
        }

        /// <summary>
        /// Gets if the color should be inverted.
        /// </summary>
        public Boolean IsInverted
        {
            get { return _color.IsInverted; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines if the given value represents a valid state of this property.
        /// </summary>
        /// <param name="value">The state that should be used.</param>
        /// <returns>True if the state is valid, otherwise false.</returns>
        protected override Boolean IsValid(CSSValue value)
        {
            var invert = Tuple.Create(Color.Transparent, true);

            return this.WithOptions(
                    this.WithBorderWidth(), 
                    this.WithLineStyle(), 
                    this.WithColor().To(m => Tuple.Create(m, false)).Or(this.TakeOne(Keywords.Invert, invert)),
                Tuple.Create(Length.Medium, LineStyle.None, invert)).TryConvert(value, m =>
                {
                    _width.SetWidth(m.Item1);
                    _style.SetStyle(m.Item2);
                    _color.SetColor(m.Item3.Item1);
                    _color.SetInverted(m.Item3.Item2);
                });
        }

        internal override String SerializeValue(IEnumerable<CSSProperty> properties)
        {
            if (!IsComplete(properties))
                return String.Empty;

            return String.Format("{0} {1} {2}", _width.SerializeValue(), _color.SerializeValue(), _style.SerializeValue());
        }

        #endregion
    }
}
