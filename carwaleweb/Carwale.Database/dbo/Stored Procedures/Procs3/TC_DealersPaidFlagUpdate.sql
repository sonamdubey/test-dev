IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealersPaidFlagUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealersPaidFlagUpdate]
GO

	

-- =============================================  
-- Author:  Vivek Gupta
-- Create date: 12-March-2013 
-- Details: SP will insert the logs of Payment of Dealer Website through CRM and update the PaidDealer field on Dealers table
-- =============================================  

CREATE PROCEDURE  [dbo].[TC_DealersPaidFlagUpdate]  
  @BranchId  AS INT,
  @PaidDealer AS BIT,
  @OprUserId AS INT
AS
  BEGIN
		
		IF @PaidDealer=1 AND NOT EXISTS (SELECT 1 FROM TC_DealerPaidLog 
		                                    WHERE BranchID=@BranchId AND IsActive=1)
		BEGIN
		 
			 UPDATE Dealers SET PaidDealer=1
			     		   WHERE ID=@BranchId;
			 
			 INSERT INTO TC_DealerPaidLog VALUES(@BranchId,GETDATE(),NULL,@OprUserId,1);  
			 
		 END 
		
		ELSE IF @PaidDealer=0
		  BEGIN
				 UPDATE Dealers SET PaidDealer=0
						   WHERE ID=@BranchId;
				 UPDATE TC_DealerPaidLog SET EndDate=GETDATE(),
												OprUserId=@OprUserId,
												IsActive=0
							               WHERE BranchID=@BranchId
							               					               
           END 
        END

