var confirmationToken = new ConfirmationToken
{
    Id = Guid.NewGuid(),
    Value = Guid.NewGuid().ToString() 
    // Etc
};

await _dbContext.ConfirmationTokens.Add(confirmationToken);
await _dbContext.SaveChangesAsync(ct);

return new ConfirmationCodeDto
{
    SvgSource = _barcodeService
        .CreateSvg(confirmationToken.Value),

    TimeRemaining = (int) _businessOptions
        .ConfirmationTokenWindow.TotalSeconds
};