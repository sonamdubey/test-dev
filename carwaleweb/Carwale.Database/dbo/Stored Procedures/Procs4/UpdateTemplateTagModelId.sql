IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateTemplateTagModelId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateTemplateTagModelId]
GO

	-- ============================================================
-- Created : Vicky Lund, 03/02/2016, Update Tag model id for a template
-- EXEC [UpdateTemplateTagModelId] 3797
-- ============================================================
CREATE  PROCEDURE [dbo].[UpdateTemplateTagModelId] @TemplateId INT
	,@ModelId INT
AS
BEGIN
	UPDATE PQ_SponsoredDealeAd_Templates
	SET TaggedModelId = @ModelId
	WHERE TemplateId = @TemplateId
END
