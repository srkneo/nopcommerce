﻿using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Models.News;

namespace Nop.Web.Validators.News
{
    public class NewsItemValidator : AbstractValidator<NewsItemModel>
    {
        public NewsItemValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.AddNewComment.CommentTitle).NotEmpty().WithMessage(localizationService.GetResource("News.Comments.CommentTitle.Required")).When(x => x.AddNewComment != null);
            RuleFor(x => x.AddNewComment.CommentText).NotEmpty().WithMessage(localizationService.GetResource("News.Comments.CommentText.Required")).When(x => x.AddNewComment != null);
        }}
}