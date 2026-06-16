//---------------------------------------------------------------------------
// File: HtmlTokenType.cs
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
// Description: Definition of token types supported by HtmlLexicalAnalyzer
//---------------------------------------------------------------------------
// Überarbeitet in angepasst für NET 8; 09/2025 Gerhard Ahrens
//---------------------------------------------------------------------------

namespace System.Windows
{
	/// <summary>
	/// types of lexical tokens for html-to-xaml converter
	/// </summary>
	internal enum HtmlTokenType
	{
		OpeningTagStart,
		ClosingTagStart,
		TagEnd,
		EmptyTagEnd,
		EqualSign,
		Name,
		Atom, // any attribute value not in quotes
		Text, //text content when accepting text
		Comment,
		EOF,
	}
}
