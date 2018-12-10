IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CV_CheckVerification_14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CV_CheckVerification_14]
GO

	
-- Modified By: Akansha 21.04.2014
-- Added SourceId Parameter
--Modified by Aditi Dhaybar 0n 29/10/14.
--Modified by Aditi Dhaybar 0n 19/11/14. Added EmailId.
CREATE PROCEDURE [dbo].[CV_CheckVerification_14.11.3] @MobileNo AS VARCHAR(50)
	,@CWICode AS VARCHAR(50)
	,@CUICode AS VARCHAR(50)
	,@IsVerified AS BIT OUTPUT
	,@SourceId AS TINYINT = 0
	,@EmailId AS VARCHAR(100)=NULL		--Modified by Aditi Dhaybar 0n 19/11/14. Added EmailId.
AS
DECLARE @Email AS VARCHAR(200)
DECLARE @CV_PendingListID AS VARCHAR(200)			--Modified by Aditi Dhaybar 0n 13/10/14. Added CV_PendingListID.

BEGIN
	SET @IsVerified = 0
	SET @Email = ''

	IF @CWICode <> ''
	BEGIN
		SELECT @CV_PendingListID = ID				--Modified by Aditi Dhaybar 0n 13/10/14. Added CV_PendingListID and WITH (NOLOCK)
			  ,@Email = Email
		FROM CV_PendingList WITH (NOLOCK)
		WHERE Mobile = @MobileNo
			AND CWICode = @CWICode  AND Email = @EmailId		--Modified by Aditi Dhaybar 0n 19/11/14. Added EmailId.
	END
	ELSE
	BEGIN
		SELECT @CV_PendingListID = ID				--Modified by Aditi Dhaybar 0n 13/10/14. Added CV_PendingListID and WITH (NOLOCK)
			  ,@Email = Email
		FROM CV_PendingList WITH (NOLOCK)
		WHERE Mobile = @MobileNo
			AND CUICode = @CUICode
	END


	IF @Email <> ''
	BEGIN
		--the lead is verified 
		--Check whether this entry is already in the CV_MobileEmailPair. If not then insert the data
	/*	SELECT EmailId
			,MobileNo
		FROM CV_MobileEmailPair WITH (NOLOCK)				--Modified by Aditi Dhaybar 0n 13/10/14. Added WITH (NOLOCK).
		WHERE EmailId = @Email
			AND MobileNo = @MobileNo

		IF @@ROWCOUNT = 0*/
		--Modified by Aditi Dhaybar 0n 29/10/14.
		IF NOT EXISTS(SELECT 1 FROM CV_MobileEmailPair WITH (NOLOCK) WHERE EmailId = @Email	AND MobileNo = @MobileNo)		
		BEGIN
			INSERT INTO CV_MobileEmailPair (
				EmailId
				,MobileNo
				,SourceId
				,CV_PendingListID					--Modified by Aditi Dhaybar 0n 13/10/14. Added CV_PendingListID.
				)
			VALUES (
				@Email
				,@MobileNo
				,@SourceId
				,@CV_PendingListID					--Modified by Aditi Dhaybar 0n 13/10/14. Added CV_PendingListID.
				)
		END

		--also make this customer as verified
		UPDATE Customers
		SET IsVerified = 1
			,IsFake = 0
		WHERE Email = @Email

		SET @IsVerified = 1
	END
	ELSE
	BEGIN
		--the code provided is wrong
		SET @IsVerified = 0
	END
END

