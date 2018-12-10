IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_CarModelGroupMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_CarModelGroupMapping]
GO

	
-- =============================================
-- Author:		Vinay Kumar
-- Create date: 01 AUG 2013
-- Description:	This proc inserts ModelId, UserId,CityId ,DateTime
-- Modifer	:	Sachin Bharti(16th Oct 2013)
-- Purpose	:	Added parameter for City Id
-- =============================================
CREATE PROCEDURE [dbo].[CRM_ADM_CarModelGroupMapping]
(   
    @GroupType INT,
    @ModelId NUMERIC, -- List of delimited items
	@dateTime DATETIME, -- List of delimited items
	@userId  NUMERIC,
	@Status INT OUTPUT,
	@CityId		INT	=	NULL	
)
AS
	BEGIN
	     IF @CityId <> -1 AND  @GroupType <> 1  --For NCD and H5000 Lead
			BEGIN 
				SELECT * FROM  CRM_ADM_GroupModelMapping AS GMM WITH(NOLOCK) 
				WHERE GMM.GroupType=@GroupType AND GMM.ModelId=@ModelId AND GMM.CityId=@CityId 
				IF @@ROWCOUNT <> 0
				BEGIN
					SET @Status = 0
				END
			ELSE
				BEGIN
					INSERT INTO CRM_ADM_GroupModelMapping(GroupType, ModelId,CreatedBy,CreatedOn,CityId) VALUES(@GroupType,@ModelId,@userId,@dateTime,@CityId)
						SET @Status = 1
				END 

	        END
		  ELSE      --for OEM Lead
			 BEGIN
				SELECT * FROM  CRM_ADM_GroupModelMapping AS GMM WITH(NOLOCK) 
				WHERE GMM.GroupType=@GroupType AND GMM.ModelId=@ModelId 
					IF @@ROWCOUNT <> 0
						BEGIN
							SET @Status = 0
						END
					ELSE
						BEGIN
							INSERT INTO CRM_ADM_GroupModelMapping(GroupType, ModelId,CreatedBy,CreatedOn,CityId) VALUES(@GroupType,@ModelId,@userId,@dateTime,@CityId)
							SET @Status = 1
						END
			  END
	END	