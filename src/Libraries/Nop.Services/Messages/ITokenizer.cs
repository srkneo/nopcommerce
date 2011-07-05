﻿using System.Collections.Generic;

namespace Nop.Services.Messages
{
    public partial interface ITokenizer
    {
        /// <summary>
        /// Replace all of the token key occurences inside the specified template text with corresponded token values
        /// </summary>
        /// <param name="template">The template with token keys inside</param>
        /// <param name="tokens">The sequence of tokens to use</param>
        /// <returns>Text with all token keys replaces by token value</returns>
        string Replace(string template, IEnumerable<Token> tokens);
    }
}
