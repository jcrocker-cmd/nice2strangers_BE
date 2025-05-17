<?php

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\ContactController;

Route::post('/send-mail', [ContactController::class, 'sendEmail']);

Route::get('/test', function () {
    return response()->json([
        'message' => 'API is working!',
        'status' => 'success',
    ]);
});

Route::post('/test-email', [ContactController::class, 'testSendEmail']);
