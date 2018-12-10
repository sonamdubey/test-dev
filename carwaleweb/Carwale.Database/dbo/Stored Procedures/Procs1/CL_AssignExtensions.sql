IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CL_AssignExtensions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CL_AssignExtensions]
GO

	-- =============================================
-- Author	:	Sachin Bharti(21st July 2014)
-- Description	:	Add dialer type for Drishti loginId extension
-- =============================================
CREATE PROCEDURE [dbo].[CL_AssignExtensions]
	@ID				SMALLINT,
	@UserId			NUMERIC,
	@ExtensionNo	NUMERIC,
	@OfficeId		SMALLINT,
	@Status			BIT OUTPUT,
	@DrishtiLogin	INT = NULL,
	@DialerType		SMALLINT 
	
 AS
	
BEGIN
	DECLARE @tempCount INT
	IF @ID = -1
		BEGIN
			--If user already exist for that Extension
			SELECT UserId FROM CL_ExtensionMap 
				WHERE Extension = @ExtensionNo
			IF @@ROWCOUNT = 0
				BEGIN
					IF (@DialerType <>3)
					BEGIN
						INSERT INTO CL_ExtensionMap(UserId, Extension, OfficeId,DrishtiLoginId,DialerType)			
								Values(@UserId, @ExtensionNo, @OfficeId,@DrishtiLogin,@DialerType)
						SET @Status =1
					END
					ELSE
						BEGIN
							--If user already exist for that DrishtiLoginId
							SELECT UserId FROM CL_ExtensionMap 
								WHERE DrishtiLoginId = @DrishtiLogin 	
							IF @@ROWCOUNT = 0
							BEGIN
								INSERT INTO CL_ExtensionMap(UserId, Extension, OfficeId,DrishtiLoginId,DialerType)			
										Values(@UserId, @ExtensionNo, @OfficeId,@DrishtiLogin,@DialerType)
								SET @Status =1
							END
							ELSE
								SET @Status =0
						END
				END
			ELSE
				SET @Status =0
		END
	ELSE
		BEGIN
			--Updation of extension 
			--If user already exist for that Extension
			SELECT UserId FROM CL_ExtensionMap 
				WHERE Extension = @ExtensionNo AND UserId <> -1
			IF @@ROWCOUNT = 0
				BEGIN
					IF (@DialerType <>3)
					BEGIN
						UPDATE CL_ExtensionMap 
						SET UserId = @UserId, OfficeId = @OfficeId,DialerType = @DialerType
						WHERE Extension = @ExtensionNo
						IF @@ROWCOUNT <> 0
							SET @Status = 1
					END
					ELSE
						BEGIN
							--If user already exist for that DrishtiLoginId
							SELECT UserId FROM CL_ExtensionMap 
							WHERE DrishtiLoginId = @DrishtiLogin AND UserId <> -1
							IF @@ROWCOUNT = 0
								BEGIN
									UPDATE CL_ExtensionMap 
									SET UserId = @UserId, OfficeId = @OfficeId, DrishtiLoginId=@DrishtiLogin,DialerType=@DialerType
									WHERE Extension = @ExtensionNo 
									IF @@ROWCOUNT <> 0
									SET @Status = 1
								END
							ELSE
								SET @Status = 0
						END
				END
			ELSE
				SET @Status =0
		END
END



