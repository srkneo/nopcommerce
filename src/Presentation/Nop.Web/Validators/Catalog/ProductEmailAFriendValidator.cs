﻿using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Models.Catalog;

namespace Nop.Web.Validators.Catalog
{
    public class ProductEmailAFriendValidator : AbstractValidator<ProductEmailAFriendModel>
    {
        public ProductEmailAFriendValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.FriendEmail).NotEmpty().WithMessage(localizationService.GetResource("Products.EmailAFriend.FriendEmail.Required"));
            RuleFor(x => x.FriendEmail).EmailAddress(); //TODO localize

            RuleFor(x => x.YourEmailAddress).NotEmpty().WithMessage(localizationService.GetResource("Products.EmailAFriend.YourEmailAddress.Required"));
            RuleFor(x => x.YourEmailAddress).EmailAddress(); //TODO localize
        }}
}