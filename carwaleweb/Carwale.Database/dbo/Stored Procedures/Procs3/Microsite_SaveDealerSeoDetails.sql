IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_SaveDealerSeoDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_SaveDealerSeoDetails]
GO

	-- =============================================
-- Author:		Sunil M. Yadav
-- Create date: 01 August 2016
-- Description:	Insert and update dealer website seo details.
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_SaveDealerSeoDetails]
	@DealerId INT 
	,@Microsite_WebsitePageId INT
	,@TitleTag VARCHAR(250) = NULL
	,@KeywordsTag VARCHAR(200) = NULL
	,@DescriptionTag VARCHAR(300) = NULL
	,@UpdatedBy INT 
AS
BEGIN
	
	IF(@DealerId > 0 AND @Microsite_WebsitePageId > 0)
	BEGIN
		-- SELECT * FROM Microsite_DealerSeoDetails
		DECLARE @Id INT = NULL,@CurrentDate DATETIME = GETDATE();
		-- SELECT @Id = Id FROM Microsite_DealerSeoDetails WITH(NOLOCK) WHERE DealerId = @DealerId

		IF NOT EXISTS(SELECT Id FROM Microsite_DealerSeoDetails WITH(NOLOCK) WHERE DealerId = @DealerId AND Microsite_WebsitePagesId = @Microsite_WebsitePageId )
		BEGIN
			INSERT INTO Microsite_DealerSeoDetails (DealerId,Microsite_WebsitePagesId,TitleTag,KeywordsTag,DescriptionTag,UpdatedBy) 
			VALUES (@DealerId,@Microsite_WebsitePageId,@TitleTag,@KeywordsTag,@DescriptionTag,@UpdatedBy)
		END
		ELSE 
			BEGIN
				UPDATE Microsite_DealerSeoDetails
				SET DealerId = @DealerId,  TitleTag = @TitleTag ,KeywordsTag = @KeywordsTag,
					DescriptionTag = @DescriptionTag , UpdatedOn = @CurrentDate , UpdatedBy = @UpdatedBy
					WHERE DealerId = @DealerId AND Microsite_WebsitePagesId = @Microsite_WebsitePageId 
			END
	END
END


