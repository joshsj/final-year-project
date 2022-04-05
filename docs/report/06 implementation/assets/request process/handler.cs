// CreateClockConfirmationCodeCommand.cs

// Create a new token
var confirmationToken = new ConfirmationToken
{
    Id = Guid.NewGuid(),
    Value = Guid.NewGuid().ToString() 
    // Etc
};

// Persist it
await _dbContext.ConfirmationTokens.Add(confirmationToken);
await _dbContext.SaveChangesAsync(ct);

// Generate a new QR code to display in the frontend
return new ConfirmationCodeDto
{
    SvgSource = _barcodeService
        .CreateSvg(confirmationToken.Value),

    TimeRemaining = (int) _businessOptions
        .ConfirmationTokenWindow.TotalSeconds
};