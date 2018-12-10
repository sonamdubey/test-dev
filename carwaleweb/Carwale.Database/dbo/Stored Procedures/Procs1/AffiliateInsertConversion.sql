IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AffiliateInsertConversion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AffiliateInsertConversion]
GO

	
CREATE PROCEDURE [dbo].[AffiliateInsertConversion]
	@SiteCode		VARCHAR(100),	
	@ConversionId		NUMERIC,	
	@CategoryId		SMALLINT,	
	@ConversionDate	DATETIME
	
	

AS
	BEGIN

		INSERT INTO 
			AffiliateConversion 
			( 
				SiteCode, 	ConversionId, 	CategoryId, 	ConversionDate 
			)
		VALUES 
			(
				@SiteCode, 	@ConversionId, @CategoryId, @ConversionDate )
					
	END
