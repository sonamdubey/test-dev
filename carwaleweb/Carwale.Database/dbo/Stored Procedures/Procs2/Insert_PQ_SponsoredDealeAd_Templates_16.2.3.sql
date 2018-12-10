IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Insert_PQ_SponsoredDealeAd_Templates_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Insert_PQ_SponsoredDealeAd_Templates_16]
GO

	-- Modified by Sourav Roy on 7th Dec 2015 
-- Modified by Chetan Thambad on 4th Feb 2016 (Inserted 'TaggedModelId' in PQ_SponsoredDealeAd_Templates)
CREATE PROCEDURE [dbo].[Insert_PQ_SponsoredDealeAd_Templates_16.2.3] 
	-- Add the parameters for the stored procedure here
	@TemplateId INT,
	@Template VARCHAR(MAX),
	@PlatformId INT,
	@CategoryId INT,
	@TaggedModelId INT = null,
	@ModifiedBy INT,
	@Status BIT OUTPUT,
	@NewTemplateId INT OUTPUT,
	@NewTemplateName VARCHAR(100) OUTPUT
AS
BEGIN
	DECLARE @Date VARCHAR(15)=CONVERT(VARCHAR(10),GETDATE(),112) + REPLACE(CONVERT(VARCHAR(10),GETDATE(),108),':','')
	IF @TemplateId = -1
	BEGIN
		DECLARE @Id INT
		INSERT INTO PQ_SponsoredDealeAd_Templates (Template,PlatformId,CategoryId,TaggedModelId)
		VALUES(@Template,@PlatformId,@CategoryId,@TaggedModelId)
		SET @Id = SCOPE_IDENTITY()
		UPDATE PQ_SponsoredDealeAd_Templates  SET TemplateName = CAST(@Id  AS VARCHAR) +'_'+ @Date,@NewTemplateName=TemplateName,@NewTemplateId=@Id 
		WHERE TemplateId = @Id
		SELECT @NewTemplateName=TemplateName FROM PQ_SponsoredDealeAd_Templates WITH(NOLOCK) 
		WHERE TemplateId = @Id
		SET @Status=1
	END
	
	ELSE
		BEGIN
			INSERT INTO PQ_SponsoredDealeAd_Template_ChangeLogs (Id,Template_Old,ModifiedDate,ModifiedBy)
			SELECT TemplateId,Template,GETDATE(),@ModifiedBy 
			FROM PQ_SponsoredDealeAd_Templates WITH(NOLOCK) 
			WHERE TemplateId = @TemplateId

			UPDATE PQ_SponsoredDealeAd_Templates  SET Template = @Template,TemplateName = CAST(@TemplateId  AS VARCHAR) +'_'+ @Date,@NewTemplateId=@TemplateId 
			WHERE TemplateId = @TemplateId     --modified by Sourav Roy
			SELECT @NewTemplateName=TemplateName 
			FROM PQ_SponsoredDealeAd_Templates WITH(NOLOCK) 
			WHERE TemplateId = @TemplateId
		SET @Status= 1
		END
END

