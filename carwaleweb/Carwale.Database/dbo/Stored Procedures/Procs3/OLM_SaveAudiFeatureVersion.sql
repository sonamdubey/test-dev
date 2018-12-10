IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SaveAudiFeatureVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SaveAudiFeatureVersion]
GO

	---Create By Vinay on 25-07-2013
CREATE  PROCEDURE [dbo].[OLM_SaveAudiFeatureVersion]
(
	@SpecId		NUMERIC(18,0),
	@VersionId	INT,
	@Value		VARCHAR(50) 
	)
	
 AS
		BEGIN
		
			SELECT Id FROM OLM_AudiBE_Version_Specs WITH (NOLOCK) WHERE VersionId = @VersionId AND SpecId=@SpecId
			IF @@ROWCOUNT = 0
				BEGIN
					
					INSERT INTO OLM_AudiBE_Version_Specs(VersionId, SpecId,Value)			
					Values(@VersionId,@SpecId,@Value )	
			  		
				END
			 ELSE
				BEGIN
				UPDATE OLM_AudiBE_Version_Specs SET Value= @Value
				WHERE SpecId = @SpecId AND VersionId=@VersionId
						
				END
		        		
		END
