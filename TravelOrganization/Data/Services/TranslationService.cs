using DeepL;
using TravelOrganization.Data.Models.Reviews;
using TravelOrganization.Data.Repositories.Reviews;

namespace TravelOrganization.Data.Services;

public class TranslationService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ITranslationRepository _translationRepository;
    private readonly ITranslator _translator;

    public TranslationService(IReviewRepository reviewRepository, ITranslationRepository translationRepository,
        ITranslator translator)
    {
        _reviewRepository = reviewRepository;
        _translationRepository = translationRepository;
        _translator = translator;
    }

    public async Task<ReviewTranslation> GetTranslation(int reviewId, string targetLanguageCode)
    {
        var translation = await _translationRepository.Get(reviewId, targetLanguageCode);

        if (translation != null)
            return translation;

        var review = await _reviewRepository.Get(reviewId);

        if (review == null) throw new ArgumentException($"Review with id {reviewId} doesn't exist");

        var translatedText = await _translator.TranslateTextAsync(new[] { review.Content }, null, targetLanguageCode);

        translation = new ReviewTranslation
        {
            ReviewId = reviewId,
            Content = translatedText[0].Text,
            LanguageCode = targetLanguageCode
        };

        await _translationRepository.Insert(translation);

        return translation;
    }
}