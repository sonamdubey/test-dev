IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveGACode]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveGACode]
GO

	
-- Created By:	Tejashree
-- Create date: 18 Aug 2014
-- Description:	Adding GA Code for a dealer
-- Modified By : Vinay Kumar prajapati Passing @DealerGACodesId =-1 for insertion and some value for deletion.
--=========================================
CREATE PROCEDURE [dbo].[TC_SaveGACode]
	@DealerId INT,
	@GACode VARCHAR(500),
	@DealerGACodesId BIGINT,
	@Status SmallInt OUTPUT
AS
BEGIN
	IF (@DealerGACodesId <> -1)
		BEGIN
		   SELECT DC.Id FROM DealerGACodes AS DC WITH(NOLOCK) WHERE DC.Code=@GACode AND DC.Id <> @DealerGACodesId
		   IF @@ROWCOUNT <> 0
			   BEGIN 
			   SET @Status=0
			   END
		   ELSE
		   BEGIN
				UPDATE	DealerGACodes 
				SET		IsActive=0 , Code=@GACode, ModifiedDate = GETDATE()
				WHERE	Id=@DealerGACodesId
				SET @Status=1
			END
		END
	ELSE
		BEGIN
			SELECT DC.Id FROM DealerGACodes AS DC WITH(NOLOCK) WHERE DC.Code=@GACode
			IF @@ROWCOUNT <> 0
				BEGIN 
					SET @Status=0
				END
			ELSE
				BEGIN
					INSERT INTO DealerGACodes (Code,DealerId,IsActive, EntryDate)
					VALUES (@GACode,@DealerId,1, GETDATE())
					SET @Status=1
				END
		END
END